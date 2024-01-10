using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PPE.Data;
using PPE.Data.Enums;
using PPE.Models;

namespace PPE.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly AppDbContext _context;

        public ProjectsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Projects
        public async Task<IActionResult> Index()
        {
              return _context.Projects != null ? 
                          View(await _context.Projects
                              .Include(e => e.Employees)
                              .ToListAsync()) :
                          Problem("Entity set 'AppDbContext.Projects'  is null.");
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Projects == null)
            {
                return NotFound();
            }
            
            var project = await _context.Projects
                .Include(e => e.ProjectStocks)
                .ThenInclude(ps => ps.PpeAttributeCategoryAttributeValue)
                .ThenInclude(paca => paca.AttributeValueAttributeCategory)
                .ThenInclude(avac => avac.AttributeValue)
                .ThenInclude(av => av.Value)
                .Include(e => e.ProjectStocks)
                .ThenInclude(ps => ps.PpeAttributeCategoryAttributeValue)
                .ThenInclude(paca => paca.Ppe)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            // group by ppe and sum stock in and stock out for each ppe 
           var projectStoks = project.ProjectStocks
               .GroupBy(s => s.PpeAttributeCategoryAttributeValue.Ppe).ToList();
           ViewBag.Labels = projectStoks.Select(s => $"{s.Key.Title}").ToList();
           ViewBag.StockIn = projectStoks.Select(s => s.Sum(s => s.QuantityIn)).ToList();
           ViewBag.StockOut = projectStoks.Select(s => s.Sum(s => s.QuantityOut)).ToList();
           ViewBag.CurrentStocks = projectStoks.Select(s => s.Sum(s => s.QuantityIn) - s.Sum(s => s.QuantityOut)).ToList();
            
            if (project == null)
            {
                return NotFound();
            }
            
            var groupedStocks = _context.Stocks
                .Where(s => s.ProjectId == id)
                .GroupBy(s => s.Ppe)
                .Select(g => new
                {
                    Ppe = g.Key,
                    StockIn = g.Where(s => s.StockType == StockType.Normal).Sum(s => s.StockIn),
                    StockOut = g.Where(s => s.StockType == StockType.Normal).Sum(s => s.StockOut),
                    CurrentStock = g.Where(s => s.StockType == StockType.Normal).Sum(s => s.StockIn) - g.Where(s => s.StockType == StockType.Normal).Sum(s => s.StockOut),
                })
                .ToList();
            ViewBag.GroupedStocks = groupedStocks;
            
            return View(project);
        }

        // GET: Projects/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Prefix,Description")] Project project)
        {
            /*return Json(new
            {
                project
            });*/
            if (ModelState.IsValid)
            {
                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Projects == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Prefix,Description")] Project project)
        {
            if (id != project.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.Id))
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
            return View(project);
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Projects == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Projects == null)
            {
                return Problem("Entity set 'AppDbContext.Projects'  is null.");
            }
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
          return (_context.Projects?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [HttpGet]
        [Route("Projects/ProjectStockDetails/{ppeId}/{projectId}")]
        public IActionResult ProjectStockDetails(int ppeId, int projectId)
        {
            var project = _context.Projects
                .Include(p => p.ProjectStocks)
                .ThenInclude(ps => ps.PpeAttributeCategoryAttributeValue)
                .ThenInclude(paca => paca.AttributeValueAttributeCategory)
                .ThenInclude(avac => avac.AttributeValue)
                .ThenInclude(av => av.Value)
                .Include(p => p.ProjectStocks)
                .ThenInclude(ps => ps.PpeAttributeCategoryAttributeValue)
                .ThenInclude(paca => paca.Ppe)
                .FirstOrDefault(p => p.Id == projectId);
            
            var projectStocks = _context.ProjectStocks
                .Where(ps => ps.PpeAttributeCategoryAttributeValue.PpeId == ppeId && ps.ProjectId == projectId)
                .GroupBy(ps => ps.PpeAttributeCategoryAttributeValue.AttributeValueAttributeCategory.AttributeValue.Value.Text)
                .Select(g => new
                {
                    Value = $"{g.Key}",
                    StockIn = g.Sum(s => s.QuantityIn),
                    StockOut = g.Sum(s => s.QuantityOut),
                    CurrentStock = g.Sum(s => s.QuantityIn) - g.Sum(s => s.QuantityOut),
                })
                .ToList();
            
            ViewBag.Labels = projectStocks.Select(s => s.Value).ToList();
            ViewBag.StockIn = projectStocks.Select(s => s.StockIn).ToList();
            ViewBag.StockOut = projectStocks.Select(s => s.StockOut).ToList();
            ViewBag.CurrentStocks = projectStocks.Select(s => s.CurrentStock).ToList();
            ViewBag.PpeTitle = project.ProjectStocks.FirstOrDefault(ps => ps.PpeAttributeCategoryAttributeValue.PpeId == ppeId)?.PpeAttributeCategoryAttributeValue.Ppe.Title;
            ViewBag.PeId = ppeId;
            ViewBag.ProjectStocks = projectStocks;
            return View(project);

        }
       
    }
}
