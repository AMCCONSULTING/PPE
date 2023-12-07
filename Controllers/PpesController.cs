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
    public class PpesController : Controller
    {
        private readonly AppDbContext _context;

        public PpesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Ppes
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Ppes.Include(p => p.Category);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Ppes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Ppes == null)
            {
                return NotFound();
            }

            var ppe = await _context.Ppes
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ppe == null)
            {
                return NotFound();
            }

            return View(ppe);
        }

        // GET: Ppes/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, 
                "Id", 
                "Title");
            return View();
        }

        // POST: Ppes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,CategoryId,Threshold")] Ppe ppe)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ppe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", 
                "Title", ppe.CategoryId);
            return View(ppe);
        }

        // GET: Ppes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Ppes == null)
            {
                return NotFound();
            }

            var ppe = await _context.Ppes.FindAsync(id);
            if (ppe == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", 
                "Title", ppe.CategoryId);
            return View(ppe);
        }

        // POST: Ppes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,CategoryId,Threshold")] Ppe ppe)
        {
            if (id != ppe.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ppe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PpeExists(ppe.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", ppe.CategoryId);
            return View(ppe);
        }

        // GET: Ppes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Ppes == null)
            {
                return NotFound();
            }

            var ppe = await _context.Ppes
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ppe == null)
            {
                return NotFound();
            }

            return View(ppe);
        }

        // POST: Ppes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Ppes == null)
            {
                return Problem("Entity set 'AppDbContext.Ppes'  is null.");
            }
            var ppe = await _context.Ppes.FindAsync(id);
            if (ppe != null)
            {
                _context.Ppes.Remove(ppe);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PpeExists(int id)
        {
          return (_context.Ppes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
