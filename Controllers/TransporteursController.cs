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
    public class TransporteursController : Controller
    {
        private readonly AppDbContext _context;

        public TransporteursController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Transporteurs
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Transporteurs.Include(t => t.Employee);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Transporteurs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Transporteurs == null)
            {
                return NotFound();
            }

            var transporteur = await _context.Transporteurs
                .Include(t => t.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transporteur == null)
            {
                return NotFound();
            }

            return View(transporteur);
        }

        // GET: Transporteurs/Create
        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FirstName");
            return View();
        }

        // POST: Transporteurs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EmployeeId")] Transporteur transporteur)
        {
           // if employeeId is already a transporteur, set error message to "Employee is already a transporteur"
           if (_context.Transporteurs.Any(t => t.EmployeeId == transporteur.EmployeeId))
            {
                ModelState.AddModelError("EmployeeId", "Employee is already a transporteur");
            }
           
           // if employeeId is null, set error message to "Employee is required"
           if (transporteur.EmployeeId == 0)
           {
               ModelState.AddModelError("EmployeeId", "Employee is required");
           }
           
            if (ModelState.IsValid)
            {
                _context.Add(transporteur);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FirstName", transporteur.EmployeeId);
            return View(transporteur);
        }

        // GET: Transporteurs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Transporteurs == null)
            {
                return NotFound();
            }

            var transporteur = await _context.Transporteurs.FindAsync(id);
            if (transporteur == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FirstName", transporteur.EmployeeId);
            return View(transporteur);
        }

        // POST: Transporteurs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EmployeeId")] Transporteur transporteur)
        {
            if (id != transporteur.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transporteur);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransporteurExists(transporteur.Id))
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
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FirstName", transporteur.EmployeeId);
            return View(transporteur);
        }

        // GET: Transporteurs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Transporteurs == null)
            {
                return NotFound();
            }

            var transporteur = await _context.Transporteurs
                .Include(t => t.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transporteur == null)
            {
                return NotFound();
            }

            return View(transporteur);
        }

        // POST: Transporteurs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Transporteurs == null)
            {
                return Problem("Entity set 'AppDbContext.Transporteurs'  is null.");
            }
            var transporteur = await _context.Transporteurs.FindAsync(id);
            if (transporteur != null)
            {
                _context.Transporteurs.Remove(transporteur);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransporteurExists(int id)
        {
          return (_context.Transporteurs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
