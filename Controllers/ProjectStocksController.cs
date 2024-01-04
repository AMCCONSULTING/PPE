using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PPE.Data;
using PPE.Models;

namespace PPE.Controllers
{
    public class ProjectStocksController : Controller
    {
        private readonly AppDbContext _context;

        public ProjectStocksController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ProjectStocks
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.ProjectStocks.Include(p => p.PpeAttributeCategoryAttributeValue).Include(p => p.Project);
            return View(await appDbContext.ToListAsync());
        }

        // GET: ProjectStocks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProjectStocks == null)
            {
                return NotFound();
            }

            var projectStock = await _context.ProjectStocks
                .Include(p => p.PpeAttributeCategoryAttributeValue)
                .Include(p => p.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectStock == null)
            {
                return NotFound();
            }

            return View(projectStock);
        }

        // GET: ProjectStocks/Create
        public IActionResult Create()
        {
            ViewData["PpeAttributeCategoryAttributeValueId"] = new SelectList(_context.PpeAttributeCategoryAttributeValues, "Id", "Id");
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Prefix");
            return View();
        }

        // POST: ProjectStocks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProjectId,PpeAttributeCategoryAttributeValueId,QuantityIn,QuantityOut,QuantityStock")] ProjectStock projectStock)
        {
            if (ModelState.IsValid)
            {
                _context.Add(projectStock);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PpeAttributeCategoryAttributeValueId"] = new SelectList(_context.PpeAttributeCategoryAttributeValues, "Id", "Id", projectStock.PpeAttributeCategoryAttributeValueId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Prefix", projectStock.ProjectId);
            return View(projectStock);
        }

        // GET: ProjectStocks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProjectStocks == null)
            {
                return NotFound();
            }

            var projectStock = await _context.ProjectStocks.FindAsync(id);
            if (projectStock == null)
            {
                return NotFound();
            }
            ViewData["PpeAttributeCategoryAttributeValueId"] = new SelectList(_context.PpeAttributeCategoryAttributeValues, "Id", "Id", projectStock.PpeAttributeCategoryAttributeValueId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Prefix", projectStock.ProjectId);
            return View(projectStock);
        }

        // POST: ProjectStocks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProjectId,PpeAttributeCategoryAttributeValueId,QuantityIn,QuantityOut,QuantityStock")] ProjectStock projectStock)
        {
            if (id != projectStock.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projectStock);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectStockExists(projectStock.Id))
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
            ViewData["PpeAttributeCategoryAttributeValueId"] = new SelectList(_context.PpeAttributeCategoryAttributeValues, "Id", "Id", projectStock.PpeAttributeCategoryAttributeValueId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Prefix", projectStock.ProjectId);
            return View(projectStock);
        }

        // GET: ProjectStocks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProjectStocks == null)
            {
                return NotFound();
            }

            var projectStock = await _context.ProjectStocks
                .Include(p => p.PpeAttributeCategoryAttributeValue)
                .Include(p => p.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectStock == null)
            {
                return NotFound();
            }

            return View(projectStock);
        }

        // POST: ProjectStocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProjectStocks == null)
            {
                return Problem("Entity set 'AppDbContext.ProjectStock'  is null.");
            }
            var projectStock = await _context.ProjectStocks.FindAsync(id);
            if (projectStock != null)
            {
                _context.ProjectStocks.Remove(projectStock);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectStockExists(int id)
        {
          return (_context.ProjectStocks?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
