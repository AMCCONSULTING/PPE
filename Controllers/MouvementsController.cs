using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataTables.AspNetCore.Mvc.Binder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PPE.Data;
using PPE.Models;

namespace PPE.Controllers
{
    public class MouvementsController : Controller
    {
        private readonly AppDbContext _context;

        public MouvementsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Mouvements
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Mouvements.Include(m => m.Coordinateur).Include(m => m.Transporteur)
                .Include(m => m.Magazinier)
                .Include(m => m.Project)
                .Include(m => m.Responsable);
            return View(await appDbContext.ToListAsync());
        }
    
         [HttpGet()]
        [Route("api/mouvements/getmouvements")]
        public IActionResult Get([DataTablesRequest] DataTablesRequest dataRequest)
        {
            IQueryable<Mouvement> mouvements = _context.Mouvements
                .Include(s => s.Magazinier)
                .Include(s => s.Responsable)
                .Include(s => s.Coordinateur)
                .Include(s => s.Project)
                .Include(s => s.Transporteur)
                .Include(s => s.MouvementDetails)
                .OrderByDescending(s => s.Date);
            
            int recordsTotal = mouvements.Count();
            int recordsFilterd = recordsTotal;

            var filters = new Dictionary<string, string>();
            foreach(var key in Request.Query.Keys)
            {
                if (key.StartsWith("filters"))
                {
                    filters[key.Substring("filters".Length)] = Request.Query[key];
                    
                    if (key == "filters[responsabele]" && !string.IsNullOrEmpty(Request.Query[key]))
                    {
                        mouvements = mouvements.Where(e => e.ResponsableId == int.Parse(Request.Query[key]));
                        recordsFilterd = mouvements.Count();
                    }

                    if (key == "filters[project]" && !string.IsNullOrEmpty(Request.Query[key]))
                    {
                        mouvements = mouvements.Where(e => e.ProjectId == int.Parse(Request.Query[key]));
                        recordsFilterd = mouvements.Count();
                    }
                    if (key == "filters[reference]" && !string.IsNullOrEmpty(Request.Query[key]))
                    {
                        mouvements = mouvements.Where(e => e.Reference.Contains(Request.Query[key].ToString()));
                        recordsFilterd = mouvements.Count();
                    }
                    if (key == "filters[date]" && !string.IsNullOrEmpty(Request.Query[key]))
                    {
                        
                        DateTime filterDate = DateTime.Parse(Request.Query[key]).Date;
                        mouvements = mouvements.Where(e => e.Date.Date == filterDate);
                        
                       // stokes = stokes.Where(e => e.Date == DateTime.Parse(Request.Query[key]));
                        recordsFilterd = mouvements.Count();
                    }
                }
            }
            
            mouvements = mouvements.Skip(dataRequest.Start).Take(dataRequest.Length);
            var deleteUrl = "api/mouvements/delete";
            
            return Json(mouvements
                .Select(e => new
                {
                    Id = e.Id,
                    Date = e.Date.ToString("d"),
                    Reference = e.Reference,
                    Responsable = e.Responsable!.Employee!.FullName,
                    Project = e.Project!.Title,
                    Magazinier = e.Magazinier!.Employee!.FullName,
                    Transporteur = e.Transporteur!.Employee!.FullName,
                    Coordinateur = e.Coordinateur!.Employee!.FullName,
                    Document = $"<a href='{e.Document}' target='_blank' class=''>" +
                               $"<i class='la la-eye'></i> Document-{e.Reference}"+
                               $"</a>",
                    Actions = $"<div class='btn-group'>" +
                              $"<a class='btn btn-success btn-sm' href='/Mouvements/AddMouvementDetails/{e.Id}'>" +
                              $"<i class='la la-plus'></i>"+
                              $"</a>"+
                              $"<a class='btn btn-info btn-sm' href='/Mouvements/Edit/{e.Id}'>" +
                              $"<i class='la la-edit'></i>"+
                              $"</a>"+
                              $"<a class='btn btn-primary btn-sm' href='/Mouvements/Details/{e.Id}'>" +
                              $"<i class='la la-eye'></i>"+
                              $"</a>"+
                              $"<a class='btn btn-danger btn-sm' onclick='confirmDelete(\"{deleteUrl}\", {e.Id})'>" +
                              $"<i class='la la-trash text-white'></i>"+
                              $"</a>"+
                              $"</div>",
                })
                .ToDataTablesResponse(dataRequest, recordsTotal, recordsFilterd));
            
        }
        
        [HttpGet]
        [Route("api/mouvements/delete/{id}")]
        public IActionResult Delete(int id)
        {
            var mouvement = _context.Mouvements.Find(id);
            if (mouvement?.MouvementDetails != null)
            {
                _context.MouvementDetails.RemoveRange(mouvement.MouvementDetails);
            }
            if (mouvement == null)
            {
                return NotFound();
            }
            _context.Mouvements.Remove(mouvement);
            _context.SaveChanges();
            return Json(new
            {
                success = true,
                message = "Mouvement deleted successfully"
            });
        }
        
        // GET: Mouvements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Mouvements == null)
            {
                return NotFound();
            }

            var mouvement = await _context.Mouvements
                .Include(m => m.Coordinateur)
                .ThenInclude(c => c.Employee)
                .Include(m => m.Transporteur)
                .ThenInclude(t => t.Employee)
                .Include(m => m.Magazinier)
                .ThenInclude(m => m.Employee)
                .Include(m => m.Project)
                .Include(m => m.Responsable)
                .ThenInclude(r => r.Employee)
                .Include(m => m.MouvementDetails)
                .ThenInclude(md => md.Article)
                .ThenInclude(a => a.AttributeValueAttributeCategory)
                .ThenInclude(avac => avac.AttributeValue)
                .ThenInclude(ac => ac.Value)
                .Include(m => m.MouvementDetails)
                .ThenInclude(md => md.Article)
                .ThenInclude(a => a.Ppe)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mouvement == null)
            {
                return NotFound();
            }

            return View(mouvement);
        }

        // GET: Mouvements/Create
        public IActionResult Create()
        {
            ViewData["CoordinateurId"] = new SelectList(_context.Coordinateurs
                .Select(r => new
                {
                    Id = r.Id,
                    FullName = r.Employee!.FullName
                }), "Id", "FullName");
            ViewData["TransporteurId"] = new SelectList(_context.Transporteurs.Select(r => new
            {
                Id = r.Id,
                FullName = r.Employee!.FullName
            }), "Id", "FullName");
            ViewData["MagazinierId"] = new SelectList(_context.Magaziniers.Select(r => new
            {
                Id = r.Id,
                FullName = r.Employee!.FullName
            }), "Id", "FullName");
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Title");
            ViewData["ResponsableId"] = new SelectList(_context.Responsables.Select(r => new
            {
                Id = r.Id,
                FullName = r.Employee!.FullName
            }), "Id", "FullName");
            return View();
        }

        // POST: Mouvements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,Reference,Document,ProjectId,ResponsableId,MagazinierId,TransporteurId,CoordinateurId")] Mouvement mouvement)
        {
            var lastMouvement = _context.Mouvements
                .OrderByDescending(m => m.Id)
                .FirstOrDefault();
            var reference = "";
            reference = lastMouvement == null ? 
                $"{mouvement.Date.Year}-M0001-P{mouvement.ProjectId:0000}" :
                $"{mouvement.Date.Year}-M{lastMouvement.Id + 1:0000}-P{mouvement.ProjectId:0000}";
            
            if (ModelState.IsValid)
            {
                var newMouvement = new Mouvement
                {
                    Date = mouvement.Date,
                    Reference = reference,
                    Document = mouvement.Document,
                    ProjectId = mouvement.ProjectId,
                    ResponsableId = mouvement.ResponsableId,
                    MagazinierId = mouvement.MagazinierId,
                    TransporteurId = mouvement.TransporteurId,
                    CoordinateurId = mouvement.CoordinateurId,
                };
                TempData["success"] = "Stoke details added successfully";

                _context.Add(newMouvement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(AddMouvementDetails), new {id = newMouvement.Id});
            }
            
            ViewData["CoordinateurId"] = new SelectList(_context.Coordinateurs.Select(r => new { Id = r.Id, FullName = r.Employee.FullName }), "Id", "FullName", mouvement.CoordinateurId);
            ViewData["TransporteurId"] = new SelectList(_context.Transporteurs.Select(r => new { Id = r.Id, FullName = r.Employee!.FullName }), "Id", "FullName");
            ViewData["MagazinierId"] = new SelectList(_context.Magaziniers.Select(r => new { Id = r.Id, FullName = r.Employee!.FullName }), "Id", "FullName", mouvement.MagazinierId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Title", mouvement.ProjectId);
            ViewData["ResponsableId"] = new SelectList(_context.Responsables.Select(r => new { Id = r.Id, FullName = r.Employee!.FullName }), "Id", "FullName", mouvement.ResponsableId);
            return View(mouvement);
        }

        // GET: Mouvements/AddMovementDetails/5
        [HttpGet]
        public async Task<IActionResult> AddMouvementDetails(int? id)
        {
            if (id == null || _context.Mouvements == null)
            {
                return NotFound();
            }

            var mouvement = await _context.Mouvements
                .Include(m => m.Coordinateur)
                .Include(m => m.Transporteur)
                .Include(m => m.Magazinier)
                .Include(m => m.Project)
                .Include(m => m.Responsable)
                .Include(m => m.MouvementDetails)!
                .ThenInclude(md => md.Article)
                .ThenInclude(a => a.AttributeValueAttributeCategory)
                .ThenInclude(avac => avac.AttributeValue)
                .ThenInclude(ac => ac.Value)
                .Include(m => m.MouvementDetails)
                .ThenInclude(md => md.Article)
                .ThenInclude(a => a.Ppe)
                .FirstOrDefaultAsync(m => m.Id == id);

           // return Json(mouvement);
            
            if (mouvement == null)
            {
                return NotFound();
            }
            ViewData["success"] = TempData["success"];
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Title");
            return View(mouvement);
        }
        
        // POST: Mouvements/AddMovementDetails/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMouvementDetails(int id, List<int> quantities, List<int> articles)
        {
            
            var mouvement = await _context.Mouvements
                .Include(m => m.Coordinateur)
                .Include(m => m.Transporteur)
                .Include(m => m.Magazinier)
                .Include(m => m.Project)
                .Include(m => m.Responsable)
                .Include(m => m.MouvementDetails)
                .FirstOrDefaultAsync(m => m.Id == id);

            using (var exception = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (id != mouvement.Id)
                    {
                        return NotFound();
                    }

                    if (ModelState.IsValid)
                    {
                        var mouvementDetails = new List<MouvementDetail>();
                        for (int i = 0; i < articles.Count; i++)
                        {
                            var mouvementDetail = new MouvementDetail
                            {
                                ArticleId = articles[i],
                                Quantity = quantities[i],
                                MouvementId = mouvement.Id
                            };
                            mouvementDetails.Add(mouvementDetail);
                            
                            var projectStock = _context.ProjectStocks
                                    .FirstOrDefault(s => s.PpeAttributeCategoryAttributeValueId == articles[i] && s.ProjectId == mouvement.ProjectId);
                            var mainStock = _context.MainStocks
                                .FirstOrDefault(s => s.PpeAttributeCategoryAttributeValueId == articles[i]);
                            
                            if (projectStock == null)
                            {
                                var newProjectStock = new ProjectStock
                                {
                                    PpeAttributeCategoryAttributeValueId = articles[i],
                                    ProjectId = mouvement.ProjectId,
                                    QuantityIn = quantities[i]
                                };
                                _context.Add(newProjectStock);
                                
                                if (mainStock != null)
                                {
                                    mainStock.QuantityOut += quantities[i];
                                    _context.Update(mainStock);
                                }
                            }
                            else
                            {
                                projectStock.QuantityIn += quantities[i];
                                _context.Update(projectStock);

                                if (mainStock != null)
                                {
                                    mainStock.QuantityOut += quantities[i];
                                    _context.Update(mainStock);
                                }
                            }
                            
                        }

                        mouvement.MouvementDetails = mouvementDetails;
                        _context.Update(mouvement);
                        await _context.SaveChangesAsync();
                        await exception.CommitAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception e)
                {
                    await exception.RollbackAsync();
                    Console.WriteLine(e);
                    throw;
                }
            }
            
           /*return Json( new
            {
                success = false,
                message = "Model state is not valid",
                errors = ModelState.Values.SelectMany(v => v.Errors)
            });*/
            TempData["success"] = "Stoke details added successfully";
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Title");
            ViewData["CoordinateurId"] = new SelectList(_context.Coordinateurs.Select(r => new { Id = r.Id, FullName = r.Employee.FullName }), "Id", "FullName", mouvement.CoordinateurId);
            ViewData["TransporteurId"] = new SelectList(_context.Transporteurs.Select(r => new { Id = r.Id, FullName = r.Employee!.FullName }), "Id", "FullName");
            ViewData["MagazinierId"] = new SelectList(_context.Magaziniers.Select(r => new { Id = r.Id, FullName = r.Employee!.FullName }), "Id", "FullName", mouvement.MagazinierId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Title", mouvement.ProjectId);
            ViewData["ResponsableId"] = new SelectList(_context.Responsables.Select(r => new { Id = r.Id, FullName = r.Employee!.FullName }), "Id", "FullName", mouvement.ResponsableId);

            return View(mouvement);
        }
        
        // GET: Mouvements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Mouvements == null)
            {
                return NotFound();
            }

            var mouvement = await _context.Mouvements.FindAsync(id);
            if (mouvement == null)
            {
                return NotFound();
            }
            ViewData["CoordinateurId"] = new SelectList(_context.Coordinateurs.Select(r => new { Id = r.Id, FullName = r.Employee.FullName }), "Id", "FullName", mouvement.CoordinateurId);
            ViewData["TransporteurId"] = new SelectList(_context.Transporteurs.Select(r => new { Id = r.Id, FullName = r.Employee!.FullName }), "Id", "FullName");
            ViewData["MagazinierId"] = new SelectList(_context.Magaziniers.Select(r => new { Id = r.Id, FullName = r.Employee!.FullName }), "Id", "FullName", mouvement.MagazinierId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Title", mouvement.ProjectId);
            ViewData["ResponsableId"] = new SelectList(_context.Responsables.Select(r => new { Id = r.Id, FullName = r.Employee!.FullName }), "Id", "FullName", mouvement.ResponsableId);

            return View(mouvement);
        }

        // POST: Mouvements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Reference,Document,ProjectId,ResponsableId,MagazinierId,TransporteurId,CoordinateurId")] Mouvement mouvement)
        {
            if (id != mouvement.Id)
            {
                return NotFound();
            }
//return Json(mouvement);
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mouvement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MouvementExists(mouvement.Id))
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
            ViewData["CoordinateurId"] = new SelectList(_context.Coordinateurs.Select(r => new { Id = r.Id, FullName = r.Employee.FullName }), "Id", "FullName", mouvement.CoordinateurId);
            ViewData["TransporteurId"] = new SelectList(_context.Transporteurs.Select(r => new { Id = r.Id, FullName = r.Employee!.FullName }), "Id", "FullName");
            ViewData["MagazinierId"] = new SelectList(_context.Magaziniers.Select(r => new { Id = r.Id, FullName = r.Employee!.FullName }), "Id", "FullName", mouvement.MagazinierId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Title", mouvement.ProjectId);
            ViewData["ResponsableId"] = new SelectList(_context.Responsables.Select(r => new { Id = r.Id, FullName = r.Employee!.FullName }), "Id", "FullName", mouvement.ResponsableId);

            return View(mouvement);
        }

        // GET: Mouvements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Mouvements == null)
            {
                return NotFound();
            }

            var mouvement = await _context.Mouvements
                .Include(m => m.Coordinateur)
                .Include(m => m.Transporteur)
                .Include(m => m.Magazinier)
                .Include(m => m.Project)
                .Include(m => m.Responsable)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mouvement == null)
            {
                return NotFound();
            }

            return View(mouvement);
        }

        // POST: Mouvements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Mouvements == null)
            {
                return Problem("Entity set 'AppDbContext.Mouvements'  is null.");
            }
            var mouvement = await _context.Mouvements.FindAsync(id);
            if (mouvement != null)
            {
                _context.Mouvements.Remove(mouvement);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MouvementExists(int id)
        {
          return (_context.Mouvements?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        
    }
}
