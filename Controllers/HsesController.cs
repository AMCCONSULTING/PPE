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
    public class HsesController : Controller
    {
        private readonly AppDbContext _context;

        public HsesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Hses
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Hses.Include(h => h.Employee);
            return View(await appDbContext.ToListAsync());
        }
        
         [HttpGet()]
        [Route("/api/get/hse/returs/{id}")]
        public IActionResult Get([DataTablesRequest] DataTablesRequest dataRequest, int id)
        {
            //IEnumerable<Ppe> ppe = _context.Ppes.Include(p => p.Category);
            IQueryable<Return> hses = _context.Returns
                .Include(p => p.Employee)
                .Include(p => p.Hse)
                .Where(p => p.HseId == id);
            
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
                    
                    /*if (!string.IsNullOrEmpty(dataRequest.Search?.Value))
                    {
                        employees = employees.Where(e => e.FirstName.Contains(dataRequest.Search.Value, StringComparison.InvariantCultureIgnoreCase));
                        recordsFilterd = employees.Count();
                    }*/
                    /*if (key == "filters[project]" && !string.IsNullOrEmpty(Request.Query[key]))
                    {
                        hses = hses.Where(e => e.ProjectId == int.Parse(Request.Query[key]));
                        recordsFilterd = hses.Count();
                    }
                    if (key == "filters[function]" && !string.IsNullOrEmpty(Request.Query[key]))
                    {
                        hses = hses.Where(e => e.FunctionId == int.Parse(Request.Query[key]));
                        recordsFilterd = hses.Count();
                    }

                    if (key == "filters[name]" && !string.IsNullOrEmpty(Request.Query[key]))
                    {
                        hses = hses.Where(e => e.FirstName.Contains(Request.Query[key].ToString()) 
                                                         || e.LastName.Contains(Request.Query[key].ToString()) 
                                                         || e.NNI.Contains(Request.Query[key].ToString()) 
                                                         || e.Phone.Contains(Request.Query[key].ToString()) 
                        );
                        recordsFilterd = hses.Count();
                    }*/
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


        // GET: Hses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Hses == null)
            {
                return NotFound();
            }

            var hse = await _context.Hses
                .Include(h => h.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hse == null)
            {
                return NotFound();
            }

            return View(hse);
        }

        // GET: Hses/Create
        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FirstName");
            return View();
        }

        // POST: Hses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EmployeeId")] Hse hse)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FirstName", hse.EmployeeId);
            return View(hse);
        }

        // GET: Hses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Hses == null)
            {
                return NotFound();
            }

            var hse = await _context.Hses.FindAsync(id);
            if (hse == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FirstName", hse.EmployeeId);
            return View(hse);
        }

        // POST: Hses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EmployeeId")] Hse hse)
        {
            if (id != hse.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HseExists(hse.Id))
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
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FirstName", hse.EmployeeId);
            return View(hse);
        }

        // GET: Hses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Hses == null)
            {
                return NotFound();
            }

            var hse = await _context.Hses
                .Include(h => h.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hse == null)
            {
                return NotFound();
            }

            return View(hse);
        }

        // POST: Hses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Hses == null)
            {
                return Problem("Entity set 'AppDbContext.Hses'  is null.");
            }
            var hse = await _context.Hses.FindAsync(id);
            if (hse != null)
            {
                _context.Hses.Remove(hse);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HseExists(int id)
        {
          return (_context.Hses?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
