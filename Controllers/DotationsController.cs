using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataTables.AspNetCore.Mvc.Binder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PPE.Data;
using PPE.Data.Enums;
using PPE.Models;

namespace PPE.Controllers
{
    public class DotationsController : Controller
    {
        private readonly AppDbContext _context;

        public DotationsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Dotations
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Dotations
                .Include(d => d.Coordinator)
                .Include(d => d.Employee)
                .Include(d => d.Magasinier)
                .OrderByDescending(d => d.Date);
                //.Include(d => d.Responsible)
                //.Include(d => d.Transporter);
            return View(await appDbContext.ToListAsync());
        }
        
        // return the value of the type of the dotation
        public string GetDotationType(TypeDotation type)
        {
            switch (type)
            {
                case TypeDotation.Dotation:
                    return "Dotation";
                case TypeDotation.Changement:
                    return "Changement";
                case TypeDotation.Reafectation:
                    return "Reafectation";
                default:
                    return "";
            }
        }
        
         [HttpGet()]
        [Route("api/dotations/getdotations")]
        public IActionResult Get([DataTablesRequest] DataTablesRequest dataRequest)
        {
            IQueryable<Dotation> mouvements = _context.Dotations
                .Include(s => s.Magasinier)
                .ThenInclude(m => m.Employee)
                //.Include(s => s.Responsible)
                .Include(s => s.Coordinator)
                .Include(s => s.Employee)
                //.Include(s => s.Transporter)
                //.ThenInclude(t => t.Employee)
                .Include(s => s.DotationDetails)
                .OrderByDescending(s => s.Date);
           // return Json(mouvements);
            int recordsTotal = mouvements.Count();
            int recordsFilterd = recordsTotal;

            var filters = new Dictionary<string, string>();
            foreach(var key in Request.Query.Keys)
            {
                if (key.StartsWith("filters"))
                {
                    filters[key.Substring("filters".Length)] = Request.Query[key];
                    
                    /*if (key == "filters[magazinier]" && !string.IsNullOrEmpty(Request.Query[key]))
                    {
                        mouvements = mouvements.Where(e => e.MagasinierId == int.Parse(Request.Query[key]));
                        recordsFilterd = mouvements.Count();
                    }*/
                    if (key == "filters[employee]" && !string.IsNullOrEmpty(Request.Query[key]))
                    {
                        mouvements = mouvements.Where(e => e.EmployeeId == int.Parse(Request.Query[key]));
                        recordsFilterd = mouvements.Count();
                    }
                    if (key == "filters[magasinier]" && !string.IsNullOrEmpty(Request.Query[key]))
                    {
                        mouvements = mouvements.Where(e => e.MagasinierId == int.Parse(Request.Query[key]));
                        recordsFilterd = mouvements.Count();
                    }
                    if (key == "filters[reference]" && !string.IsNullOrEmpty(Request.Query[key]))
                    {
                        mouvements = mouvements.Where(e => e.Reference.Contains(Request.Query[key].ToString()));
                        recordsFilterd = mouvements.Count();
                    }
                    
                    if (key == "filters[startDate]" && !string.IsNullOrEmpty(Request.Query[key]))
                    {
                        DateTime startDate = DateTime.Parse(Request.Query[key]).Date;

                        // Assuming you have an endDate parameter in your query string
                        if (Request.Query.ContainsKey("filters[endDate]") && !string.IsNullOrEmpty(Request.Query["filters[endDate]"]))
                        {
                            DateTime endDate = DateTime.Parse(Request.Query["filters[endDate]"]).Date.AddDays(1); // Add one day to include records on the end date

                            mouvements = mouvements.Where(e => e.Date.Date >= startDate && e.Date.Date < endDate);
                            recordsFilterd = mouvements.Count();
                        }
                        else
                        {
                            // If endDate is not specified, filter records only for the start date
                            mouvements = mouvements.Where(e => e.Date.Date == startDate);
                            recordsFilterd = mouvements.Count();
                        }
                    }
                    
                }
            }
            
            mouvements = mouvements.Skip(dataRequest.Start).Take(dataRequest.Length);
            var deleteUrl = "api/dotations/delete";
            
            return Json(mouvements
                .Select(e => new
                {
                    Id = e.Id,
                    Date = e.Date.ToString("d"),
                    Reference = e.Reference,
                    Employee = e.Employee!.FullName,
                    //Responsable = e.Responsible!.Employee!.FullName,
                    Coordinateur = e.Coordinator!.Employee!.FullName,
                    //Transporteur = e.Transporter!.Employee!.FullName,
                    Magasinier = e.Magasinier!.Employee!.FullName,
                    TypeDotation = $"<strong class='text-uppercase badge {GetBadgeClass(e.Type)}'>{e.Type.ToString()}</strong>",
                    Document = GetDocumentLink(e.Document),
                    Actions = $"<div class='btn-group'>" +
                              $"<a class='btn btn-success btn-sm' href='/Dotations/AddDotationDetails/{e.Id}'>" +
                              $"<i class='la la-plus'></i>"+
                              $"</a>"+
                              $"<a class='btn btn-info btn-sm' href='/Dotations/Edit/{e.Id}'>" +
                              $"<i class='la la-edit'></i>"+
                              $"</a>"+
                              $"<a class='btn btn-primary btn-sm' href='/Dotations/Details/{e.Id}'>" +
                              $"<i class='la la-eye'></i>"+
                              $"</a>"+
                              $"<a class='btn btn-danger btn-sm' onclick='confirmDelete(\"{deleteUrl}\", {e.Id})'>" +
                              $"<i class='la la-trash text-white'></i>"+
                              $"</a>"+
                              $"</div>",
                })
                .ToDataTablesResponse(dataRequest, recordsTotal, recordsFilterd));
            
        }

        private static string GetDocumentLink(string? document)
        {
            if (document.IsNullOrEmpty())
            {
                return string.Empty;
            }
            return $"<a href='{document}' target='_blank' class=''>" +
                   $"<i class='la la-eye'></i> Document"+
                   $"</a>";
        }
        
        private static string GetBadgeClass(TypeDotation type)
        {
            switch (type)
            {
                case TypeDotation.Dotation:
                    return "badge-success";
                case TypeDotation.Changement:
                    return "badge-info";
                case TypeDotation.Reafectation:
                    return "badge-danger";
                default:
                    return string.Empty;
            }
        }
        
        // GET: Dotations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Dotations == null)
            {
                return NotFound();
            }

            var dotation = await _context.Dotations
                .Include(d => d.Coordinator)
                .ThenInclude(c => c.Employee)
                .Include(d => d.Employee)
                .Include(d => d.Magasinier)
                .ThenInclude(m => m.Employee)
                /*.Include(d => d.Responsible)
                .ThenInclude(r => r.Employee)
                .Include(d => d.Transporter)
                .ThenInclude(t => t.Employee)*/
                .Include(d => d.DotationDetails)!
                .ThenInclude(dd => dd.Article)
                .ThenInclude(a => a.Ppe)
                .Include(p => p.DotationDetails)
                .ThenInclude(dd => dd.Article)
                .ThenInclude(pacav => pacav.AttributeValueAttributeCategory)
                .ThenInclude(avac => avac.AttributeValue)
                .ThenInclude(av => av.Value)
                
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dotation == null)
            {
                return NotFound();
            }

            return View(dotation);
        }

        // GET: Dotations/Create
        [HttpGet]
        [Route("Dotations/Create/{employeeId}")]
        public IActionResult Create(int employeeId)
        {
           // return Json(new { employeeId = employeeId });
            var employee = _context.Employees
                .FirstOrDefault(e => e.Id == employeeId);
            if (employee == null)
            {
                return NotFound();
            }
            
            ViewData["Employee"] = employee;
            
            ViewData["CoordinatorId"] = new SelectList(_context.Coordinateurs.Select(r => new { Id = r.Id, FullName = r.Employee!.FullName }), "Id", "FullName");
            ViewData["EmployeeId"] = employeeId.ToString();
            ViewData["MagasinierId"] = new SelectList(_context.Magaziniers.Select(r => new { Id = r.Id, FullName = r.Employee!.FullName }), "Id", "FullName");
            ViewData["ResponsibleId"] = new SelectList(_context.Responsables.Select(r => new { Id = r.Id, FullName = r.Employee!.FullName }), "Id", "FullName");
            ViewData["TransporterId"] = new SelectList(_context.Transporteurs.Select(r => new { Id = r.Id, FullName = r.Employee!.FullName }), "Id", "FullName");
            return View();
        }

        // POST: Dotations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,Reference,EmployeeId,Document,CoordinatorId,MagasinierId,Type,IsFromStock")] Dotation dotation)
        {
            //dotation.IsFromStock

           // return Json(dotation);
            
            var lastDotation = _context.Dotations
                .OrderByDescending(d => d.Id)
                .FirstOrDefault();

            dotation.Reference = lastDotation == null ? $"{DateTime.Now.Year}-DOT{1:0000}-EMP{dotation.EmployeeId}" : $"{DateTime.Now.Year}-DOT{lastDotation.Id + 1:0000}-EMP{dotation.EmployeeId}";
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (ModelState.IsValid)
                {
                    var newDotation = new Dotation
                    {
                        Date = dotation.Date,
                        Reference = dotation.Reference,
                        EmployeeId = dotation.EmployeeId,
                        CoordinatorId = dotation.CoordinatorId,
                        MagasinierId = dotation.MagasinierId,
                        Type = dotation.Type,
                        Document = dotation.Document,
                        IsFromStock = dotation.IsFromStock
                    };
                
                    var entry = _context.Entry(newDotation);
                    if (entry.State != EntityState.Added)
                    {
                        _context.Entry(newDotation).State = EntityState.Added;
                    }
                    await _context.SaveChangesAsync();
                    return RedirectToAction("AddDotationDetails", new { id = newDotation.Id });
                }
            } 
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                Console.WriteLine(e);
                throw;
            }
            
            ViewData["CoordinatorId"] = new SelectList(_context.Coordinateurs, "Id", "Id", dotation.CoordinatorId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FirstName", dotation.EmployeeId);
            ViewData["MagasinierId"] = new SelectList(_context.Magaziniers, "Id", "Id", dotation.MagasinierId);
            return View(dotation);
        }

        // GET: Dotations/Edit/5
        [HttpGet]
        //[Route("Dotations/Edit/{id}/{employeeId}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Dotations == null)
            {
                return NotFound();
            }

            var dotation = await _context.Dotations.FindAsync(id);
            if (dotation == null)
            {
                return NotFound();
            }
            ViewBag.EmployeeId = dotation.EmployeeId;
            ViewData["CoordinatorId"] = new SelectList(_context.Coordinateurs.Select(r => new { Id = r.Id, FullName = r.Employee!.FullName }), "Id", "FullName", dotation.CoordinatorId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", dotation.EmployeeId);
            ViewData["MagasinierId"] = new SelectList(_context.Magaziniers.Select(r => new { Id = r.Id, FullName = r.Employee!.FullName }), "Id", "FullName", dotation.MagasinierId);
            /*ViewData["ResponsibleId"] = new SelectList(_context.Responsables.Select(r => new { Id = r.Id, FullName = r.Employee!.FullName }), "Id", "FullName", dotation.ResponsibleId);
            ViewData["TransporterId"] = new SelectList(_context.Transporteurs.Select(r => new { Id = r.Id, FullName = r.Employee!.FullName }), "Id", "FullName", dotation.TransporterId);*/
            return View(dotation);
        }

        // POST: Dotations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       // [Route("Dotations/Edit/{id}/{employeeId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Reference,EmployeeId,Document,CoordinatorId,MagasinierId,Type")] Dotation dotation)
        {
            if (id != dotation.Id)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dotation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DotationExists(dotation.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.EmployeeId = dotation.EmployeeId;
            ViewData["CoordinatorId"] = new SelectList(_context.Coordinateurs, "Id", "Id", dotation.CoordinatorId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FirstName", dotation.EmployeeId);
            ViewData["MagasinierId"] = new SelectList(_context.Magaziniers, "Id", "Id", dotation.MagasinierId);
            /*ViewData["ResponsibleId"] = new SelectList(_context.Responsables, "Id", "Id", dotation.ResponsibleId);
            ViewData["TransporterId"] = new SelectList(_context.Transporteurs, "Id", "Id", dotation.TransporterId);*/
            return View(dotation);
        }

        // GET: Dotations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Dotations == null)
            {
                return NotFound();
            }

            var dotation = await _context.Dotations
                .Include(d => d.Coordinator)
                .Include(d => d.Employee)
                .Include(d => d.Magasinier)
                /*.Include(d => d.Responsible)
                .Include(d => d.Transporter)*/
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dotation == null)
            {
                return NotFound();
            }

            return View(dotation);
        }

        // POST: Dotations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Dotations == null)
            {
                return Problem("Entity set 'AppDbContext.Dotations'  is null.");
            }
            var dotation = await _context.Dotations.FindAsync(id);
            if (dotation != null)
            {
                _context.Dotations.Remove(dotation);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DotationExists(int id)
        {
          return (_context.Dotations?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [HttpGet]
        //[Route("Dotations/AddDotationDetails/{id}")]
        public IActionResult AddDotationDetails(int id)
        {
            var dotation = _context.Dotations
                .Include(d => d.Employee)
                .Include(d => d.DotationDetails)!
                .ThenInclude(dd => dd.Article)
                .ThenInclude(a => a.Ppe)
                .Include(d => d.DotationDetails)
                .ThenInclude(dd => dd.Article)
                .ThenInclude(pacav => pacav.AttributeValueAttributeCategory)
                .ThenInclude(avac => avac.AttributeValue)
                .ThenInclude(av => av.Value)
                .FirstOrDefault(d => d.Id == id);
            if (dotation == null)
            {
                //return Json(new { success = false, message = "Dotation not found" });
                return NotFound();
            }
            
            ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Title");
            return View(dotation);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Dotations/AddDotationDetails/{id}")]
        public async Task<IActionResult> AddDotationDetails(int id, List<int> articles, List<int> quantities)
        {
            var dotation = _context.Dotations
                .Include(d => d.Employee)
                .Include(d => d.DotationDetails)!
                .ThenInclude(dd => dd.Article)
                .ThenInclude(a => a.Ppe)
                .FirstOrDefault(d => d.Id == id);
            
            //return Json(new { articles = articles, quantities = quantities });

            if (dotation.IsFromStock)
            {
                return Json(
                    new
                    {
                        success = false,
                        message = "This dotation is from the stock normal",
                    });
            }
            else
            {
                return Json(new
                {
                    success = true,
                    message = "This dotation is from project stock",
                });
            }

            if (ModelState.IsValid)
            {
                if (dotation == null)
                {
                    return NotFound();
                }

                await using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    for (int i = 0; i < articles.Count; i++)
                    {
                        var article = _context.ProjectStocks
                            .Include(ps => ps.PpeAttributeCategoryAttributeValue)
                            .FirstOrDefault(ps => ps.PpeAttributeCategoryAttributeValueId == articles[i]);

                        var employeeStock = _context.StockEmployees
                            .FirstOrDefault(se => se.EmployeeId == dotation.EmployeeId && se.ArticleId == articles[i]);

                        if (article == null)
                        {
                            return Json(new
                            {
                                message = "The article is not found",
                            });
                        }
                        else
                        {
                            if (employeeStock == null)
                            {
                                var newEmployeeStock = new StockEmployee
                                {
                                    ArticleId = articles[i],
                                    EmployeeId = dotation.EmployeeId,
                                    Quantity = quantities[i]
                                };
                                _context.StockEmployees.Add(newEmployeeStock);

                                // remove the article from the project stock
                                article!.QuantityOut += quantities[i];
                                _context.ProjectStocks.Update(article);

                                // add the dotations details to the dotation
                                var dotationDetail = new DotationDetail
                                {
                                    ArticleId = articles[i],
                                    Quantity = quantities[i],
                                    DotationId = id,
                                };
                                _context.DotationDetails.Add(dotationDetail);
                            }
                            else
                            {
                                // the employee has already this article in his stock
                                employeeStock.Quantity += quantities[i];
                                _context.StockEmployees.Update(employeeStock);
                                
                                // remove the article from the project stock
                                article!.QuantityOut += quantities[i];
                                _context.ProjectStocks.Update(article);
                                
                                // add the dotations details to the dotation
                                var dotationDetail = new DotationDetail
                                {
                                    ArticleId = articles[i],
                                    Quantity = quantities[i],
                                    DotationId = id,
                                };
                                _context.DotationDetails.Add(dotationDetail);
                            }
                        }
                        
                        // if type is donation is reafectation we add the article to payableStock
                        if (dotation.Type == TypeDotation.Reafectation)
                        {
                            var payableStock = _context.PayableStocks
                                .FirstOrDefault(ps => ps.ArticleId == articles[i] && ps.EmployeeId == dotation.EmployeeId);
                            if (payableStock == null)
                            {
                                var newPayableStock = new PayableStock
                                {
                                    ArticleId = articles[i],
                                    Quantity = quantities[i],
                                    EmployeeId = dotation.EmployeeId,
                                };
                                _context.PayableStocks.Add(newPayableStock);
                            }
                            else
                            {
                                payableStock.Quantity += quantities[i];
                                _context.PayableStocks.Update(payableStock);
                            }
                        }
                    }
                    
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    TempData["success"] = $"Succussfully affected to {dotation.Employee.FullName}";
                    return RedirectToAction("Details", "Employees",new { id = dotation.EmployeeId });
                }
                catch (Exception e)
                {
                    await transaction.RollbackAsync();
                    return Json(new
                    {
                        message = e.Message,
                    });
                    Console.WriteLine(e);
                    
                    return BadRequest();
                }
                
            }
            ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Title");
            return View(dotation);
        }
        
        [HttpDelete]
        [Route("/api/remove/article/{id}/dotation/{dotationId}")]
        public async Task<IActionResult> RemoveArticle(int id, int dotationId)
        {
            var dotationDetail = _context.DotationDetails
                .Include(dd => dd.Dotation)
                .FirstOrDefault(dd => dd.DotationId == dotationId && dd.ArticleId == id);
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // remove the article from the employee stock
                var employeeStock = _context.StockEmployees
                    .FirstOrDefault(se => se.EmployeeId == dotationDetail.Dotation.EmployeeId && se.ArticleId == id);
                if (employeeStock != null)
                {
                    employeeStock.Quantity -= dotationDetail.Quantity;
                    _context.StockEmployees.Update(employeeStock);
                }
                
                // return the article to the project stock
                var article = _context.ProjectStocks
                    .FirstOrDefault(ps => ps.PpeAttributeCategoryAttributeValueId == id);
                if (article != null)
                {
                    article.QuantityOut -= dotationDetail.Quantity;
                    _context.ProjectStocks.Update(article);
                }
                
                // remove article from dotations details
                _context.DotationDetails.Remove(dotationDetail);
                
                // if type is donation is reafectation we remove the article from payableStock
                if (dotationDetail.Dotation.Type == TypeDotation.Reafectation)
                {
                    var payableStock = _context.PayableStocks
                        .FirstOrDefault(ps => ps.ArticleId == id && ps.EmployeeId == dotationDetail.Dotation.EmployeeId);
                    if (payableStock != null)
                    {
                        payableStock.Quantity -= dotationDetail.Quantity;
                        _context.PayableStocks.Update(payableStock);
                    }
                }
                await _context.SaveChangesAsync();
               await transaction.CommitAsync();
               return Json(new
               {
                   success = true,
                   message = "Deleted successfully"
               });
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                Console.WriteLine(e);
                throw;
            }
            
        }
        
        // api/dotations/delete/9
        [HttpGet]
        [Route("api/dotations/delete/{id}")]
        public IActionResult Delete(int id)
        {
            var dotation = _context.Dotations.FirstOrDefault(d => d.Id == id);
            _context.Dotations.Remove(dotation);
            _context.SaveChanges();

            return Json(new
            {
                success = true,
                message = "Deleted successfully"
            });
        }
        
    }
}
