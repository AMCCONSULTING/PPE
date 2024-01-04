using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
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
    public class ReturnsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly string _tenantId = "2bb37157-5ae3-40a9-b823-fb07fcaeaffa";
        private readonly string _clientId = "d6d87276-54a4-49fb-8cbd-cd530655ae12";
        private readonly string _clientSecret = "a5P8Q~CZVEQQIPt_gAMzJclVEUkpQ.4tL783Qdrx";
        private readonly string[] _scopes = { "https://graph.microsoft.com/.default" };

        public ReturnsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Returns
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Returns
                .Include(re => re.Employee)
                .Include(re => re.Responsable)
                .Include(re => re.Hse)
                .Include(re => re.Magazinier)
                .Include(re => re.Article);
            return View(await appDbContext.ToListAsync());
        }
        
         [HttpGet()]
        [Route("api/returns/getdotations")]
        public IActionResult Get([DataTablesRequest] DataTablesRequest dataRequest)
        {
            IQueryable<Return> mouvements = _context.Returns
                .Include(s => s.Magazinier)
                .ThenInclude(m => m.Employee)
                .Include(s => s.Responsable)
                .ThenInclude(t => t.Employee)
                .Include(s => s.Hse)
                .ThenInclude(t => t.Employee)
                .Include(s => s.Article)
                .ThenInclude(t => t.Ppe)
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
                    if (key == "filters[employee]" && !string.IsNullOrEmpty(Request.Query[key]))
                    {
                        mouvements = mouvements.Where(e => e.EmployeeId == int.Parse(Request.Query[key]));
                        recordsFilterd = mouvements.Count();
                    }
                    if (key == "filters[magasinier]" && !string.IsNullOrEmpty(Request.Query[key]))
                    {
                        mouvements = mouvements.Where(e => e.MagazinierId == int.Parse(Request.Query[key]));
                        recordsFilterd = mouvements.Count();
                    }
                    if (key == "filters[hse]" && !string.IsNullOrEmpty(Request.Query[key]))
                    {
                        mouvements = mouvements.Where(e => e.HseId == int.Parse(Request.Query[key]));
                        recordsFilterd = mouvements.Count();
                    }
                    if (key == "filters[responsable]" && !string.IsNullOrEmpty(Request.Query[key]))
                    {
                        mouvements = mouvements.Where(e => e.ResponsableId == int.Parse(Request.Query[key]));
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
                    if (key == "filters[status]" && !string.IsNullOrEmpty(Request.Query[key]))
                    {
                        mouvements = mouvements.Where(e => e.Status == (RetunStatus)Enum.Parse(typeof(RetunStatus), Request.Query[key]));
                        recordsFilterd = mouvements.Count();
                    }
                    // filter by ppe
                    if (key == "filters[ppe]" && !string.IsNullOrEmpty(Request.Query[key]))
                    {
                        mouvements = mouvements.Where(e => e.Article!.PpeId == int.Parse(Request.Query[key]));
                        recordsFilterd = mouvements.Count();
                    }
                }
            }
            
            mouvements = mouvements.Skip(dataRequest.Start).Take(dataRequest.Length);
            var deleteUrl = "api/returns/delete";
            
            return Json(mouvements
                .Select(e => new
                {
                    Id = e.Id,
                    Date = e.Date.ToString("d"),
                    Article = e.Article!.Ppe.Title,
                    Employee = e.Employee!.FullName,
                    Responsable = e.Responsable!.Employee!.FullName,
                    Magasinier = e.Magazinier!.Employee!.FullName,
                    Hse = e.Hse != null ? e.Hse.Employee!.FullName : string.Empty,
                    Status = $"<strong class='text-uppercase badge {GetBadgeClass(e.Status)}'>{e.Status.ToString()}</strong>",
                    Document = GetDocumentLink(e.Document),
                    quantity = e.Quantity,
                    Actions = $"<div class='btn-group'>" +
                              /*$"<a class='btn btn-success btn-sm' href='#/Dotations/AddDotationDetails/{e.Id}'>" +
                              $"<i class='la la-plus'></i>"+
                              $"</a>"+*/
                              $"<a class='btn btn-info btn-sm' href='/Returns/Edit/{e.Id}'>" +
                              $"<i class='la la-edit'></i>"+
                              $"</a>"+
                              $"<a class='btn btn-primary btn-sm' href='/Returns/Details/{e.Id}'>" +
                              $"<i class='la la-eye'></i>"+
                              $"</a>"+
                              $"<a class='btn btn-danger btn-sm' onclick=#'confirmDelete(\"{deleteUrl}\", {e.Id})'>" +
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

        private static string GetBadgeClass(object type)
        {
            return type switch
            {
                RetunStatus.Good => "badge-success",
                RetunStatus.Lost => "badge-danger",
                _ => "badge-info"
            };
        }


        // GET: Returns/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Returns == null)
            {
                return NotFound();
            }

            var epeRetun = _context.Returns
                .Include(re => re.Employee)
                .Include(re => re.Responsable)
                .Include(re => re.Hse)
                .Include(re => re.Magazinier)
                .Include(re => re.Article);
               
               return View(await epeRetun.FirstOrDefaultAsync(m => m.Id == id));
        }

        // GET: Returns/Create
        [HttpGet]
        [Route("/Returns/Create/{articleId?}/{employeeId?}")]
        public IActionResult Create(int? articleId, int? employeeId)
        {
            
            if (articleId == null || employeeId == null)
            {
                return NotFound();
            }
            
            ViewData["ArticleId"] = articleId;
            ViewData["EmployeeId"] = employeeId;
            ViewData["HseId"] = new SelectList(_context.Hses, "Id", "Id");
            ViewData["MagazinierId"] = new SelectList(_context.Magaziniers, "Id", "Id");
            ViewData["ResponsableId"] = new SelectList(_context.Responsables, "Id", "Id");
            return View();
        }

        // POST: Returns/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Route("/Returns/Create/{id?}")]
        public async Task<IActionResult> Create([Bind("Id,EmployeeId,ArticleId,ResponsableId,HseId,MagazinierId,Quantity,IsPaid,Document,Status")] Return ppeRetun)
        {
            
            var articleInStockEmployee = _context.StockEmployees
                .FirstOrDefault(ps => ps.EmployeeId == ppeRetun.EmployeeId && ps.ArticleId == ppeRetun.ArticleId);

            var clientSecretCredential = new ClientSecretCredential(_tenantId, _clientId, _clientSecret);
            var tokenRequestContext = new TokenRequestContext(_scopes);

            var accessToken = await clientSecretCredential.GetTokenAsync(tokenRequestContext);
            
           // return Json(accessToken);
            
            if (articleInStockEmployee == null)
            {
                return NotFound();
            }
            
            if (articleInStockEmployee.Quantity < ppeRetun.Quantity)
            {
                ModelState.AddModelError("Quantity", "La quantité demandée est supérieure à la quantité avec l'employé.");
            }
            else
            {
                articleInStockEmployee.Quantity -= ppeRetun.Quantity;
                _context.StockEmployees.Update(articleInStockEmployee);
            }
            
            if (ModelState.IsValid)
            {
                _context.Add(ppeRetun);
                await _context.SaveChangesAsync();
               
                return RedirectToAction("Details", "Employees", new {id = ppeRetun.EmployeeId});
            }
            ViewData["ArticleId"] =  ppeRetun.ArticleId;
            ViewData["EmployeeId"] = ppeRetun.EmployeeId;
            ViewData["HseId"] = new SelectList(_context.Hses, "Id", "Id", ppeRetun.HseId);
            ViewData["MagazinierId"] = new SelectList(_context.Magaziniers, "Id", "Id", ppeRetun.MagazinierId);
            ViewData["ResponsableId"] = new SelectList(_context.Responsables, "Id", "Id", ppeRetun.ResponsableId);
            return View(ppeRetun);
        }

        // GET: Returns/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            
            if (id == null || _context.Returns == null)
            {
                return NotFound();
            }
            var @return = await _context.Returns
                .Include(re => re.Employee)
                .Include(re => re.Responsable)
                .Include(re => re.Hse)
                .Include(re => re.Magazinier)
                .Include(re => re.Article)
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (@return == null)
            {
                return NotFound();
            }
            
            
            ViewData["ArticleId"] = @return.ArticleId;
            ViewData["EmployeeId"] = @return.EmployeeId;
            ViewData["HseId"] = new SelectList(_context.Hses, "Id", "Id", @return.HseId);
            ViewData["MagazinierId"] = new SelectList(_context.Magaziniers, "Id", "Id", @return.MagazinierId);
            ViewData["ResponsableId"] = new SelectList(_context.Responsables, "Id", "Id", @return.ResponsableId);
            return View(@return);
        }

        // POST: Returns/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EmployeeId,ArticleId,ResponsableId,HseId,MagazinierId,Quantity,IsPaid,Status")] Return @return)
        {
            if (id != @return.Id)
            {
                return NotFound();
            }
            
            var articleInStockEmployee = _context.StockEmployees
                .FirstOrDefault(ps => ps.EmployeeId == @return.EmployeeId && ps.ArticleId == @return.ArticleId);
          
            // find last return for this article and employee and delete it from stock employee
            var lastReturn = _context.Returns
                .Where(r => r.ArticleId == @return.ArticleId && r.EmployeeId == @return.EmployeeId)
                .OrderByDescending(r => r.Date)
                .FirstOrDefault();
            if (lastReturn != null)
            {
                articleInStockEmployee!.Quantity += lastReturn.Quantity;
                _context.StockEmployees.Update(articleInStockEmployee);

                _context.Returns.Remove(lastReturn);
                await _context.SaveChangesAsync();
            }
            await _context.SaveChangesAsync();
            
            if (articleInStockEmployee == null)
            {
                ModelState.AddModelError("Quantity", "Not found.");
                return NotFound();
            }
            
            if (articleInStockEmployee.Quantity < @return.Quantity)
            {
                ModelState.AddModelError("Quantity", "La quantité demandée est supérieure à la quantité avec l'employé.");
            }
            else
            {
                articleInStockEmployee.Quantity -= @return.Quantity;
                _context.StockEmployees.Update(articleInStockEmployee);
                await _context.SaveChangesAsync();
            }
            

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@return);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReturnExists(@return.Id))
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
            ViewData["ArticleId"] = @return.ArticleId;
            ViewData["EmployeeId"] = @return.EmployeeId;
            ViewData["HseId"] = new SelectList(_context.Hses, "Id", "Id", @return.HseId);
            ViewData["MagazinierId"] = new SelectList(_context.Magaziniers, "Id", "Id", @return.MagazinierId);
            ViewData["ResponsableId"] = new SelectList(_context.Responsables, "Id", "Id", @return.ResponsableId);
            return View(@return);
        }

        // GET: Returns/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Returns == null)
            {
                return NotFound();
            }

            return NotFound();
        }

        // POST: Returns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Returns == null)
            {
                return Problem("Entity set 'AppDbContext.Returns'  is null.");
            }
            var @return = await _context.Returns.FindAsync(id);
            if (@return != null)
            {
                _context.Returns.Remove(@return);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReturnExists(int id)
        {
          return (_context.Returns?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public IActionResult StockReturn()
        {
            
            var appDbContext = _context.Returns
                .Include(re => re.Employee)
                .Include(re => re.Responsable)
                .Include(re => re.Hse)
                .Include(re => re.Magazinier)
                .Include(re => re.Article)
                .ThenInclude(a => a.Ppe)
                .Include(re => re.Article)
                .ThenInclude(a => a.AttributeValueAttributeCategory)
                .ThenInclude(a => a.AttributeValue)
                .ThenInclude(a => a.Value)
                .Where(r => r.Status != RetunStatus.Lost);
            return View(appDbContext.ToList());
            
            //return View();
        }
    }
}
