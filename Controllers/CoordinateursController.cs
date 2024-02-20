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
    public class CoordinateursController : Controller
    {
        private readonly AppDbContext _context;

        public CoordinateursController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Coordinateurs
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Coordinateurs.Include(c => c.Employee);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Coordinateurs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Coordinateurs == null)
            {
                return NotFound();
            }

            var coordinateur = await _context.Coordinateurs
                .Include(c => c.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (coordinateur == null)
            {
                return NotFound();
            }

            return View(coordinateur);
        }

        // GET: Coordinateurs/Create
        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FirstName");
            return View();
        }

        // POST: Coordinateurs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EmployeeId")] Coordinateur coordinateur)
        {
            
            // if employee is already a coordinateur, return error
            if (_context.Coordinateurs.Any(c => c.EmployeeId == coordinateur.EmployeeId))
            {
                ModelState.AddModelError("EmployeeId", "Employee is already a coordinateur");
            }
            
            // if employeeId is null, set error message to "Employee is required"
            if (coordinateur.EmployeeId == 0)
            {
                ModelState.AddModelError("EmployeeId", "Employee is required");
            }
            
            if (ModelState.IsValid)
            {
                _context.Add(coordinateur);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FirstName", coordinateur.EmployeeId);
            return View(coordinateur);
        }

        // GET: Coordinateurs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Coordinateurs == null)
            {
                return NotFound();
            }

            var coordinateur = await _context.Coordinateurs.FindAsync(id);
            if (coordinateur == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FirstName", coordinateur.EmployeeId);
            return View(coordinateur);
        }

        // POST: Coordinateurs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EmployeeId")] Coordinateur coordinateur)
        {
            if (id != coordinateur.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(coordinateur);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CoordinateurExists(coordinateur.Id))
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
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FirstName", coordinateur.EmployeeId);
            return View(coordinateur);
        }

        // GET: Coordinateurs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Coordinateurs == null)
            {
                return NotFound();
            }

            var coordinateur = await _context.Coordinateurs
                .Include(c => c.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (coordinateur == null)
            {
                return NotFound();
            }

            return View(coordinateur);
        }

        // POST: Coordinateurs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Coordinateurs == null)
            {
                return Problem("Entity set 'AppDbContext.Coordinateurs'  is null.");
            }
            var coordinateur = await _context.Coordinateurs.FindAsync(id);
            if (coordinateur != null)
            {
                _context.Coordinateurs.Remove(coordinateur);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CoordinateurExists(int id)
        {
          return (_context.Coordinateurs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
