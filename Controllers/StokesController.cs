using DataTables.AspNetCore.Mvc.Binder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PPE.Data;
using PPE.Data.Services;
using PPE.Models;

namespace PPE.Controllers
{
    public class StokesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IStokeService _stokeService;
        
        public StokesController(AppDbContext context, IStokeService stokeService)
        {
            _context = context;
            _stokeService = stokeService;
        }

        // GET: Stokes
        public async Task<IActionResult> Index()
        {
            var stokes = await _stokeService.GetStocks();
            return View(stokes);
        }
        
        // GET: Stokes/GetStokes
        [HttpGet()]
        [Route("api/stokes/getstokes")]
        public IActionResult Get([DataTablesRequest] DataTablesRequest dataRequest)
        {
            
            IQueryable<Stoke> stokes = _context.Stokes
                .Include(s => s.Magazinier)
                .Include(s => s.Responsable)
                .Include(s => s.StokeDetails)
                .OrderByDescending(s => s.Date);
            
            int recordsTotal = stokes.Count();
            int recordsFilterd = recordsTotal;

            var filters = new Dictionary<string, string>();
            foreach(var key in Request.Query.Keys)
            {
                if (key.StartsWith("filters"))
                {
                    filters[key.Substring("filters".Length)] = Request.Query[key];
                    
                    if (key == "filters[responsabele]" && !string.IsNullOrEmpty(Request.Query[key]))
                    {
                        stokes = stokes.Where(e => e.ResponsableId == int.Parse(Request.Query[key]));
                        recordsFilterd = stokes.Count();
                    }
                    if (key == "filters[reference]" && !string.IsNullOrEmpty(Request.Query[key]))
                    {
                        stokes = stokes.Where(e => e.Reference.Contains(Request.Query[key].ToString()));
                        recordsFilterd = stokes.Count();
                    }
                    /*if (key == "filters[date]" && !string.IsNullOrEmpty(Request.Query[key]))
                    {
                        
                        int filterDate = DateTime.Parse(Request.Query[key]).Date.Month;
                        stokes = stokes.Where(e => e.Date.Date.Month == filterDate);
                        
                       // stokes = stokes.Where(e => e.Date == DateTime.Parse(Request.Query[key]));
                        recordsFilterd = stokes.Count();
                    }*/
                    if (key == "filters[startDate]" && !string.IsNullOrEmpty(Request.Query[key]))
                    {
                        DateTime startDate = DateTime.Parse(Request.Query[key]).Date;

                        // Assuming you have an endDate parameter in your query string
                        if (Request.Query.ContainsKey("filters[endDate]") && !string.IsNullOrEmpty(Request.Query["filters[endDate]"]))
                        {
                            DateTime endDate = DateTime.Parse(Request.Query["filters[endDate]"]).Date.AddDays(1); // Add one day to include records on the end date
                            stokes = stokes.Where(e => e.Date.Date >= startDate && e.Date.Date < endDate);
                            recordsFilterd = stokes.Count();
                        }
                        else
                        {
                            // If endDate is not specified, filter records only for the start date
                            stokes = stokes.Where(e => e.Date.Date >= startDate);
                            recordsFilterd = stokes.Count();
                        }
                    }
                }
            }
            
            stokes = stokes.Skip(dataRequest.Start).Take(dataRequest.Length);
            var deleteUrl = "Stokes/DeleteStoke";
            
            return Json(stokes
                .Select(e => new
                {
                    Id = e.Id,
                    Date = e.Date.ToString("d"),
                    Reference = e.Reference,
                    Responsable = e.Responsable!.Employee!.FullName,
                    Magazinier = e.Magazinier!.Employee!.FullName,
                    Document = $"<a href='{e.Document}' target='_blank' class=''>" +
                               $"<i class='la la-eye'></i> Document-{e.Reference}"+
                               $"</a>",
                    Actions = $"<div class='btn-group'>" +
                              $"<a class='btn btn-success btn-sm' href='/Stokes/AddStokeDetails/{e.Id}'>" +
                              $"<i class='la la-plus'></i>"+
                              $"</a>"+
                              $"<a class='btn btn-info btn-sm' href='/Stokes/Edit/{e.Id}'>" +
                              $"<i class='la la-edit'></i>"+
                              $"</a>"+
                              $"<a class='btn btn-primary btn-sm' href='/Stokes/Details/{e.Id}'>" +
                              $"<i class='la la-eye'></i>"+
                              $"</a>"+
                              $"<a class='btn btn-danger btn-sm' onclick='confirmDelete(\"{deleteUrl}\", {e.Id})'>" +
                              $"<i class='la la-trash text-white'></i>"+
                              $"</a>"+
                              $"</div>",
                })
                .ToDataTablesResponse(dataRequest, recordsTotal, recordsFilterd));
            
        }

        // GET: Stokes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Stokes == null)
            {
                return NotFound();
            }

            var stoke = await _stokeService.GetStoke(id.GetValueOrDefault());
            if (stoke == null)
            {
                return NotFound();
            }

            return View(stoke);
        }

        // GET: Stokes/Create
        public IActionResult Create()
        {
            ViewData["MagazinierId"] = new SelectList(_context.Magaziniers
                .Select(m => new
                {
                    Id = m.Id,
                    FullName = m.Employee.FullName
                }), "Id", "FullName");
            ViewData["ResponsableId"] = new SelectList(_context.Responsables
                .Select(r => new
                {
                    Id = r.Id,
                    FullName = r.Employee.FullName
                }), "Id", "FullName");
            return View();
        }

        // POST: Stokes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,Reference,Document,MagazinierId,ResponsableId")] Stoke stoke)
        {
            // generate reference
            var lastStock = await _context.Stokes.OrderByDescending(s => s.Id).FirstOrDefaultAsync();
            stoke.Reference = lastStock != null ? $"{DateTime.Now.Year}-{lastStock.Id + 1:0000}" : $"{DateTime.Now.Year}-{1:0000}";
            
            if (ModelState.IsValid)
            {
                await _stokeService.AddStoke(stoke);
                return RedirectToAction(nameof(AddStokeDetails) , new { id = stoke.Id });
            }
            
            ViewData["MagazinierId"] = new SelectList(_context.Magaziniers
                .Select(m => new
                {
                    Id = m.Id,
                    FullName = m.Employee.FullName
                }), "Id", "FullName");
            ViewData["ResponsableId"] = new SelectList(_context.Responsables
                .Select(r => new
                {
                    Id = r.Id,
                    FullName = r.Employee.FullName
                }), "Id", "FullName");
            return View(stoke);
        }
        
        // GET: Stokes/AddStokeDetails/5
        [HttpGet]
        public async Task<IActionResult> AddStokeDetails(int? id)
        {
            if (id == null || _context.Stokes == null)
            {
                return NotFound();
            }

            var stoke = await _context.Stokes
                    .Include(s => s.Magazinier)
                    .Include(s => s.Responsable)
                    .Include(s => s.StokeDetails)!
                    .ThenInclude(sd => sd.Article)
                    .ThenInclude(a => a.Ppe)
                    .Include(s => s.StokeDetails)
                    .ThenInclude(sd => sd.Article)
                    .ThenInclude(a => a.AttributeValueAttributeCategory)
                    .ThenInclude(avac => avac.AttributeValue)
                    .ThenInclude(av => av.Value)
                    .FirstOrDefaultAsync(m => m.Id == id);
            
           // return Json(stoke);
            
            if (stoke == null)
            {
                return NotFound();
            }
            ViewData["success"] = TempData["success"];
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Title");
            //return Json(ViewBag.Categories);
            return View(stoke);
        }
        
        // POST: Stokes/AddStokeDetails/5
        [HttpPost]
        /*[ValidateAntiForgeryToken]*/
        public async Task<IActionResult> AddStokeDetails(int id, List<int> articles, List<int> quantities)
        {
            
            if (id == null || _context.Stokes == null)
            {
                return NotFound();
            }
            

            if (ModelState.IsValid)
            {
                await using var exeption = await _context.Database.BeginTransactionAsync();
                try
                {
                    for (int i = 0; i < articles.Count; i++)
                    {
                        var article = await _context.StokeDetails
                            .FirstOrDefaultAsync(sd => sd.StokeId == id && sd.ArticleId == articles[i]);
                        var mainStock = await _context.MainStocks
                            .FirstOrDefaultAsync(ms => ms.PpeAttributeCategoryAttributeValueId == articles[i]);
                        if (article == null)
                        {
                            await _stokeService.AddStokeDetails(new StokeDetail
                            {
                                StokeId = id,
                                ArticleId = articles[i],
                                Quantity = quantities[i]
                            });
                            if (mainStock == null)
                            {
                                await _stokeService.AddMainStock(new MainStock
                                {
                                    PpeAttributeCategoryAttributeValueId = articles[i],
                                    QuantityIn = quantities[i]
                                });
                            } else
                            {
                                mainStock.QuantityIn += quantities[i];
                                await _stokeService.UpdateMainStock(mainStock);
                            }
                        }
                        else
                        {
                            var mainStockQuantityIn = mainStock!.QuantityIn;
                            var newQuantityIn = mainStockQuantityIn - article.Quantity;
                            article.Quantity = quantities[i];
                            await _stokeService.UpdateStokeDetails(article);
                            mainStock.QuantityIn = newQuantityIn + quantities[i];
                            await _stokeService.UpdateMainStock(mainStock);
                        }
                        
                    }
                    await exeption.CommitAsync();
                    TempData["success"] = "Stoke details added successfully";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception e)
                {
                    await exeption.RollbackAsync();
                    return Problem(e.Message);
                }
            }
            
            var stoke = await _context.Stokes
                .Include(s => s.Magazinier)
                .Include(s => s.Responsable)
                .Include(s => s.StokeDetails)!
                .ThenInclude(sd => sd.Article)
                .ThenInclude(a => a.Ppe)
                .Include(s => s.StokeDetails)
                .ThenInclude(sd => sd.Article)
                .ThenInclude(a => a.AttributeValueAttributeCategory)
                .ThenInclude(avac => avac.AttributeValue)
                .ThenInclude(av => av.Value)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Title");
            
            return View(stoke);
        }

        // GET: Stokes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Stokes == null)
            {
                return NotFound();
            }

            var stoke = await _context.Stokes.FindAsync(id);
            if (stoke == null)
            {
                return NotFound();
            }
            ViewData["MagazinierId"] = new SelectList(_context.Magaziniers
                .Select(m => new
                {
                    Id = m.Id,
                    FullName = m.Employee.FullName
                }), "Id", "FullName");
            ViewData["ResponsableId"] = new SelectList(_context.Responsables
                .Select(r => new
                {
                    Id = r.Id,
                    FullName = r.Employee.FullName
                }), "Id", "FullName");
            return View(stoke);
        }

        // POST: Stokes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Reference,Document,MagazinierId,ResponsableId")] Stoke stoke)
        {
            if (id != stoke.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _stokeService.UpdateStoke(stoke);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StokeExists(stoke.Id))
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
            ViewData["MagazinierId"] = new SelectList(_context.Magaziniers
                .Select(m => new
                {
                    Id = m.Id,
                    FullName = m.Employee!.FullName
                }), "Id", "FullName");
            ViewData["ResponsableId"] = new SelectList(_context.Responsables
                .Select(r => new
                {
                    Id = r.Id,
                    FullName = r.Employee!.FullName
                }), "Id", "FullName");
            return View(stoke);
        }

        // GET: Stokes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Stokes == null)
            {
                return NotFound();
            }

            var stoke = await _context.Stokes
                .Include(s => s.Magazinier)
                .Include(s => s.Responsable)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stoke == null)
            {
                return NotFound();
            }

            return View(stoke);
        }

        // POST: Stokes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Stokes == null)
            {
                return Problem("Entity set 'AppDbContext.Stokes'  is null.");
            }
            var stoke = await _context.Stokes.FindAsync(id);
            if (stoke != null)
            {
                _context.Stokes.Remove(stoke);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        // GET: Stokes/DeleteStoke/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Route("Stokes/DeleteStoke/{id}")]
        public async Task<IActionResult> DeleteStoke(int? id)
        {
            
            if (id == null || _context.Stokes == null)
            {
                return NotFound();
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var stoke = await _context.Stokes.FindAsync(id);
                    if (stoke == null)
                    {
                        return NotFound();
                    }

                    _context.Stokes.Remove(stoke);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return Json(new { success = true, message = "Stoke deleted successfully" });
                }
                catch (Exception e)
                {
                    await transaction.RollbackAsync();
                    return Problem(e.Message);
                }
            }
        }

        private bool StokeExists(int id)
        {
          return (_context.Stokes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
