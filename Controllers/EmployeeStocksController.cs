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
    public class EmployeeStocksController : Controller
    {
        private readonly AppDbContext _context;

        public EmployeeStocksController(AppDbContext context)
        {
            _context = context;
        }

        // GET: EmployeeStocks
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.EmployeeStocks.Include(e => e.Employee).Include(e => e.VariantValue);
            return View(await appDbContext.ToListAsync());
        }

        // GET: EmployeeStocks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.EmployeeStocks == null)
            {
                return NotFound();
            }

            var employeeStock = await _context.EmployeeStocks
                .Include(e => e.Employee)
                .Include(e => e.VariantValue)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employeeStock == null)
            {
                return NotFound();
            }

            return View(employeeStock);
        }

        // GET: EmployeeStocks/Create
        public IActionResult Create()
        {
            ViewData["Status"] = new SelectList(Enum.GetValues(typeof(StockEmployeeStatus)));
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FirstName");
            ViewData["VariantValueId"] = new SelectList(_context.VariantValues, "Id", "Id");
            return View();
        }

        // POST: EmployeeStocks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,StockIn,StockOut,Status,Remarks,VariantValueId,EmployeeId")] EmployeeStock employeeStock)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employeeStock);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Status"] = new SelectList(Enum.GetValues(typeof(StockEmployeeStatus)));
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FirstName", employeeStock.EmployeeId);
            ViewData["VariantValueId"] = new SelectList(_context.VariantValues, "Id", "Id", employeeStock.VariantValueId);
            return View(employeeStock);
        }

        // GET: EmployeeStocks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.EmployeeStocks == null)
            {
                return NotFound();
            }

            var employeeStock = await _context.EmployeeStocks.FindAsync(id);
            if (employeeStock == null)
            {
                return NotFound();
            }
            ViewData["Status"] = new SelectList(Enum.GetValues(typeof(StockEmployeeStatus)));
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FirstName", employeeStock.EmployeeId);
            ViewData["VariantValueId"] = new SelectList(_context.VariantValues, "Id", "Id", employeeStock.VariantValueId);
            return View(employeeStock);
        }

        // POST: EmployeeStocks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,StockIn,StockOut,Status,Remarks,VariantValueId,EmployeeId")] EmployeeStock employeeStock)
        {
            if (id != employeeStock.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employeeStock);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeStockExists(employeeStock.Id))
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
            ViewData["Status"] = new SelectList(Enum.GetValues(typeof(StockEmployeeStatus)));
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FirstName", employeeStock.EmployeeId);
            ViewData["VariantValueId"] = new SelectList(_context.VariantValues, "Id", "Id", employeeStock.VariantValueId);
            return View(employeeStock);
        }

        // GET: EmployeeStocks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.EmployeeStocks == null)
            {
                return NotFound();
            }

            var employeeStock = await _context.EmployeeStocks
                .Include(e => e.Employee)
                .Include(e => e.VariantValue)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employeeStock == null)
            {
                return NotFound();
            }

            return View(employeeStock);
        }

        // POST: EmployeeStocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.EmployeeStocks == null)
            {
                return Problem("Entity set 'AppDbContext.EmployeeStocks'  is null.");
            }
            var employeeStock = await _context.EmployeeStocks.FindAsync(id);
            if (employeeStock != null)
            {
                _context.EmployeeStocks.Remove(employeeStock);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeStockExists(int id)
        {
          return (_context.EmployeeStocks?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
