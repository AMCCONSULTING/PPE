using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataTables.AspNetCore.Mvc.Binder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PPE.Data;
using PPE.Data.Enums;
using PPE.Models;

namespace PPE.Controllers
{
    public class MagaziniersController : Controller
    {
        private readonly AppDbContext _context;

        public MagaziniersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Magaziniers
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Magaziniers.Include(m => m.Employee);
            return View(await appDbContext.ToListAsync());
        }
        
            [HttpGet()]
        [Route("/api/get/magazinier/returs/{id}")]
        public IActionResult Get([DataTablesRequest] DataTablesRequest dataRequest, int id)
        {
            //IEnumerable<Ppe> ppe = _context.Ppes.Include(p => p.Category);
            IQueryable<Return> hses = _context.Returns
                .Include(p => p.Employee)
                .Include(p => p.Hse)
                .Where(p => p.MagazinierId == id);
            
            int recordsTotal = hses.Count();
            int recordsFilterd = recordsTotal;

            var filters = new Dictionary<string, string>();
            foreach(var key in Request.Query.Keys)
            {
                if (key.StartsWith("filters"))
                {
                    filters[key.Substring("filters".Length)] = Request.Query[key];
                    if (key == "filters[employee]" && !string.IsNullOrEmpty(Request.Query[key]))
                    {
                        hses = hses.Where(e => e.EmployeeId == int.Parse(Request.Query[key]));
                        recordsFilterd = hses.Count();
                    }
                    if (key == "filters[ppe]" && !string.IsNullOrEmpty(Request.Query[key]))
                    {
                        hses = hses.Where(e => e.Article.Ppe.Id == int.Parse(Request.Query[key]));
                        recordsFilterd = hses.Count();
                    }
                    if (key == "filters[status]" && !string.IsNullOrEmpty(Request.Query[key]))
                    {
                        /*hses = hses.Where(e => e.Status.ToString() == Request.Query[key]);
                        recordsFilterd = hses.Count();*/
                        if (Enum.TryParse(Request.Query[key], out RetunStatus statusFilter))
                        {
                            hses = hses.Where(e => e.Status == statusFilter);
                            recordsFilterd = hses.Count();
                        }
                        else
                        {
                            // Handle the case where the provided status value is not a valid enum value
                            // You might want to log an error or handle it according to your application's requirements
                        }

                    }
                    
                }
                if (key == "filters[startDate]" && !string.IsNullOrEmpty(Request.Query[key]))
                {
                    DateTime startDate = DateTime.Parse(Request.Query[key]).Date;

                    // Assuming you have an endDate parameter in your query string
                    if (Request.Query.ContainsKey("filters[endDate]") && !string.IsNullOrEmpty(Request.Query["filters[endDate]"]))
                    {
                        DateTime endDate = DateTime.Parse(Request.Query["filters[endDate]"]).Date.AddDays(1); // Add one day to include records on the end date

                        hses = hses.Where(e => e.Date.Date >= startDate && e.Date.Date < endDate);
                        recordsFilterd = hses.Count();
                    }
                    else
                    {
                        // If endDate is not specified, filter records only for the start date
                        hses = hses.Where(e => e.Date.Date == startDate);
                        recordsFilterd = hses.Count();
                    }
                }

            }
            
            hses = hses.Skip(dataRequest.Start).Take(dataRequest.Length);
            
            return Json(hses
                .Select(e => new
                {
                    Id = e.Id,
                    Employee = e.Employee!.FullName,
                    Ppe = e.Article!.Ppe.Title,
                    Status = e.Status.ToString(),
                    Date = e.Date.ToString("dd/MM/yyyy"),
                    Quantity = e.Quantity,
                })
                .ToDataTablesResponse(dataRequest, recordsTotal, recordsFilterd));
            // date
            /*ppe
                quantity
            employee
                status*/
        }


        // GET: Magaziniers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Magaziniers == null)
            {
                return NotFound();
            }

            var magazinier = await _context.Magaziniers
                .Include(m => m.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (magazinier == null)
            {
                return NotFound();
            }

            return View(magazinier);
        }

        // GET: Magaziniers/Create
        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FirstName");
            return View();
        }

        // POST: Magaziniers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EmployeeId")] Magazinier magazinier)
        {
            // if employeeId is already a magazinier, set error message to "Employee is already a magazinier"
            if (_context.Magaziniers.Any(t => t.EmployeeId == magazinier.EmployeeId))
            {
                ModelState.AddModelError("EmployeeId", "Employee is already a magazinier");
            }
            // employeeIs is null, set error message to "Employee is required"
            if (magazinier.EmployeeId == 0)
            {
                ModelState.AddModelError("EmployeeId", "Employee is required");
            }
            if (ModelState.IsValid)
            {
                _context.Add(magazinier);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FirstName", magazinier.EmployeeId);
            return View(magazinier);
        }

        // GET: Magaziniers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Magaziniers == null)
            {
                return NotFound();
            }

            var magazinier = await _context.Magaziniers.FindAsync(id);
            if (magazinier == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FirstName", magazinier.EmployeeId);
            return View(magazinier);
        }

        // POST: Magaziniers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EmployeeId")] Magazinier magazinier)
        {
            if (id != magazinier.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(magazinier);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MagazinierExists(magazinier.Id))
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
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FirstName", magazinier.EmployeeId);
            return View(magazinier);
        }

        // GET: Magaziniers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Magaziniers == null)
            {
                return NotFound();
            }

            var magazinier = await _context.Magaziniers
                .Include(m => m.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (magazinier == null)
            {
                return NotFound();
            }

            return View(magazinier);
        }

        // POST: Magaziniers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Magaziniers == null)
            {
                return Problem("Entity set 'AppDbContext.Magaziniers'  is null.");
            }
            var magazinier = await _context.Magaziniers.FindAsync(id);
            if (magazinier != null)
            {
                _context.Magaziniers.Remove(magazinier);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MagazinierExists(int id)
        {
          return (_context.Magaziniers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
