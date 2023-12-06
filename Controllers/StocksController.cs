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
    public class StocksController : Controller
    {
        private readonly AppDbContext _context;

        public StocksController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Stocks
        public async Task<IActionResult> Index()
        {
            // Define your filter values
            var projectId = 1/* your project id */;
            var ppeId = 1/* your ppe id */;
            var variantValueId = 1/* your variant value id */;

            /*var appDbContext = _context.Stocks
                .Where(s =>
                    (projectId == 0 || s.Project.Id == projectId) &&
                    //(ppeId == 0 || s.VariantValue.Variant.Ppe.Id == ppeId) &&
                    (variantValueId == 0 || s.VariantValue.Id == variantValueId))
                .Include(s => s.Project)
                .Include(s => s.VariantValue)
                .ThenInclude(s => s.Variant)
                .ThenInclude(s => s.Ppe);*/
            var appDbContext = _context.Stocks
                .Include(s => s.Project)
                .Include(s => s.VariantValue)
                .ThenInclude(s => s.Variant)
                .ThenInclude(s => s.Ppe);
            
            return View(await appDbContext.ToListAsync());
        }

        // GET: Stocks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Stocks == null)
            {
                return NotFound();
            }

            var stock = await _context.Stocks
                .Include(s => s.Project)
                .Include(s => s.VariantValue)
                .ThenInclude(s => s.Variant)
                .ThenInclude(s => s.Ppe)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stock == null)
            {
                return NotFound();
            }

            return View(stock);
        }

        // GET: Stocks/Create
        public IActionResult Create()
        {
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Prefix");
            ViewData["VariantValueId"] = _context.VariantValues
                .Include(v => v.Variant)
                .ThenInclude(v => v.Ppe)
                .Select(v => new SelectListItem
                {
                    Text = $"{v.Variant.Ppe.Title} - {v.Value}",
                    Value = v.Id.ToString(),
                });
            return View();
        }
        
        // POST: Stocks/SaveStocks
        [HttpPost]
        public IActionResult SaveStocks(List<Stock> stocks)
        {
            Console.WriteLine($"Stocks: {stocks}");
            return Json(stocks);
            try
            {
                foreach (var stock in stocks)
                {
                    // Save each stock entry to the database
                    var newStock = new Stock
                    {
                        VariantValueId = stock.VariantValueId,
                        StockIn = stock.StockIn,
                        StockOut = stock.StockOut,
                        ProjectId = stock.ProjectId,
                        Date = stock.Date,
                        // Add other properties as needed
                    };

                    _context.Stocks.Add(newStock);
                }

                _context.SaveChanges();

                return RedirectToAction("Index"); // Redirect to another action or page
            }
            catch (Exception ex)
            {
                // Handle exceptions (log or display an error message)
                return View("Error");
            }
        }

        // POST: Stocks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Quantity,Date,ProjectId,StockIn,StockOut,VariantValueId")] Stock stock)
        {

            return Json(stock);
            
            if (ModelState.IsValid)
            {
                // if project already has a stock for this variant value, update it instead of creating a new one
                var existingStock = await _context.Stocks
                    .FirstOrDefaultAsync(s => s.ProjectId == stock.ProjectId && s.VariantValueId == stock.VariantValueId);
                if (existingStock != null)
                {
                    existingStock.StockIn += stock.StockIn;
                    existingStock.StockOut += stock.StockOut;
                    _context.Update(existingStock);
                }
                else
                {
                    _context.Add(stock); 
                }
               
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Prefix", stock.ProjectId);
            ViewData["VariantValueId"] = _context.VariantValues
                .Include(v => v.Variant)
                .ThenInclude(v => v.Ppe)
                .Select(v => new SelectListItem
                {
                    Text = $"{v.Variant.Ppe.Title} - {v.Value}",
                    Value = v.Id.ToString(),
                    Selected = v.Id == stock.VariantValueId,
                });
            return View(stock);
        }
        
        // GET: Stocks/GetVariantValueNotInProjectStocks/5
        public async Task<IActionResult> GetVariantValueNotInProjectStocks(int? id)
        {
            if (id == null || _context.Stocks == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            var variantValues = _context.VariantValues
                .Include(v => v.Variant)
                .ThenInclude(v => v.Ppe)
                .Where(v => !_context.Stocks.Any(s => s.VariantValueId == v.Id && s.ProjectId == id))
                .Select(v => new SelectListItem
                {
                    Text = $"{v.Variant.Ppe.Title} - {v.Value}",
                    Value = v.Id.ToString(),
                });
            
            var options = "<option value=''>-- Select an option --</option>";
            foreach (var variantValue in variantValues)
            {
                options += $"<option " + $"value='{variantValue.Value}'>{variantValue.Text}</option>";
            }
            
            return Json(options);
        }

        // GET: Stocks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Stocks == null)
            {
                return NotFound();
            }

            var stock = await _context.Stocks.FindAsync(id);
            if (stock == null)
            {
                return NotFound();
            }
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Prefix", stock.ProjectId);
            ViewData["VariantValueId"] = _context.VariantValues
                .Include(v => v.Variant)
                .ThenInclude(v => v.Ppe)
                .Select(v => new SelectListItem
                {
                    Text = $"{v.Variant.Ppe.Title} - {v.Value}",
                    Value = v.Id.ToString(),
                    Selected = v.Id == stock.VariantValueId,
                });
            
            //return Json(ViewData["VariantValueId"]);
            return View(stock);
        }

        // POST: Stocks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Quantity,Date,ProjectId,StockIn,StockOut,VariantValueId")] Stock stock)
        {
            if (id != stock.Id)
            {
                return NotFound();
            }

            //return Json(stock);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stock);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StockExists(stock.Id))
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

            //return Json(ModelState.Values.SelectMany(v => v.Errors));
            
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Prefix", stock.ProjectId);
            ViewData["VariantValueId"] = 
                //new SelectList(_context.VariantValues, "Id", "Id", stock.VariantValueId);
                _context.VariantValues
                    .Include(v => v.Variant)
                    .ThenInclude(v => v.Ppe)
                    .Select(v => new SelectListItem
                    {
                        Text = $"{v.Variant.Ppe.Title} - {v.Value}",
                        Value = v.Id.ToString(),
                        Selected = v.Id == stock.VariantValueId,
                    });
            return View(stock);
        }

        // GET: Stocks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Stocks == null)
            {
                return NotFound();
            }

            var stock = await _context.Stocks
                .Include(s => s.Project)
                .Include(s => s.VariantValue)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stock == null)
            {
                return NotFound();
            }

            return View(stock);
        }

        // POST: Stocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Stocks == null)
            {
                return Problem("Entity set 'AppDbContext.Stocks'  is null.");
            }
            var stock = await _context.Stocks.FindAsync(id);
            if (stock != null)
            {
                _context.Stocks.Remove(stock);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StockExists(int id)
        {
          return (_context.Stocks?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public IActionResult FilterStock()
        {
            throw new NotImplementedException();
        }
    }
}
