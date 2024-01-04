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
    public class ResponsablesController : Controller
    {
        private readonly AppDbContext _context;

        public ResponsablesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Responsables
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Responsables.Include(r => r.Employee);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Responsables/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Responsables == null)
            {
                return NotFound();
            }

            var responsable = await _context.Responsables
                .Include(r => r.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (responsable == null)
            {
                return NotFound();
            }

            return View(responsable);
        }

        // GET: Responsables/Create
        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FirstName");
            return View();
        }

        // POST: Responsables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EmployeeId")] Responsable responsable)
        {
            if (ModelState.IsValid)
            {
                _context.Add(responsable);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FirstName", responsable.EmployeeId);
            return View(responsable);
        }

        // GET: Responsables/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Responsables == null)
            {
                return NotFound();
            }

            var responsable = await _context.Responsables.FindAsync(id);
            if (responsable == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FirstName", responsable.EmployeeId);
            return View(responsable);
        }

        // POST: Responsables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EmployeeId")] Responsable responsable)
        {
            if (id != responsable.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(responsable);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ResponsableExists(responsable.Id))
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
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FirstName", responsable.EmployeeId);
            return View(responsable);
        }

        // GET: Responsables/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Responsables == null)
            {
                return NotFound();
            }

            var responsable = await _context.Responsables
                .Include(r => r.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (responsable == null)
            {
                return NotFound();
            }

            return View(responsable);
        }

        // POST: Responsables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Responsables == null)
            {
                return Problem("Entity set 'AppDbContext.Responsables'  is null.");
            }
            var responsable = await _context.Responsables.FindAsync(id);
            if (responsable != null)
            {
                _context.Responsables.Remove(responsable);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ResponsableExists(int id)
        {
          return (_context.Responsables?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
