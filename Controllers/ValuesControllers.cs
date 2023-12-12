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
    public class ValuesControllers : Controller
    {
        private readonly AppDbContext _context;

        public ValuesControllers(AppDbContext context)
        {
            _context = context;
        }

        // GET: ValuesControllers
        public async Task<IActionResult> Index()
        {
              return _context.Values != null ? 
                          View(await _context.Values.ToListAsync()) :
                          Problem("Entity set 'AppDbContext.Values'  is null.");
        }

        // GET: ValuesControllers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Values == null)
            {
                return NotFound();
            }

            var value = await _context.Values
                .FirstOrDefaultAsync(m => m.Id == id);
            if (value == null)
            {
                return NotFound();
            }

            return View(value);
        }

        // GET: ValuesControllers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ValuesControllers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Text")] Value value)
        {
            if (ModelState.IsValid)
            {
                _context.Add(value);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(value);
        }

        // GET: ValuesControllers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Values == null)
            {
                return NotFound();
            }

            var value = await _context.Values.FindAsync(id);
            if (value == null)
            {
                return NotFound();
            }
            return View(value);
        }

        // POST: ValuesControllers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Text")] Value value)
        {
            if (id != value.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(value);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ValueExists(value.Id))
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
            return View(value);
        }

        // GET: ValuesControllers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Values == null)
            {
                return NotFound();
            }

            var value = await _context.Values
                .FirstOrDefaultAsync(m => m.Id == id);
            if (value == null)
            {
                return NotFound();
            }

            return View(value);
        }

        // POST: ValuesControllers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Values == null)
            {
                return Problem("Entity set 'AppDbContext.Values'  is null.");
            }
            var value = await _context.Values.FindAsync(id);
            if (value != null)
            {
                _context.Values.Remove(value);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ValueExists(int id)
        {
          return (_context.Values?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
