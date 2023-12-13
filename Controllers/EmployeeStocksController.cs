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
            var appDbContext = _context.EmployeeStocks.Include(e => e.Employee);
                //.Include(e => e.VariantValue);
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
              //  .Include(e => e.VariantValue)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employeeStock == null)
            {
                return NotFound();
            }

            return View(employeeStock);
        }

        // GET: EmployeeStocks/Create
        public IActionResult Create(int? id)
        {
            
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }
            
            var employee = _context.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }
            
            ViewBag.Employee = employee;
            
            ViewBag.Category = ViewBag.Category = _context.Categories
                .Include(c => c.Ppes)
                .ThenInclude(p => p.PpeAttributeCategoryAttributeValues)
                .ThenInclude(p => p.StockDetails)
                .ThenInclude(p => p.Stock)
                .Where(c => c.Ppes.Any(p => p.PpeAttributeCategoryAttributeValues.Any(p => p.StockDetails.Any(s => s.Stock.ProjectId == employee.ProjectId))))
                .ToList();
            ViewBag.Status = new SelectList(Enum.GetValues(typeof(StockEmployeeStatus)));
            ViewData["ProjectId"] = employee.ProjectId.ToString();
            ViewData["FunctionId"] = employee.FunctionId.ToString();
            ViewData["EmployeeId"] = employee.Id.ToString();

            //return Json(ViewBag.Category);
            return View();
        }

        // POST: EmployeeStocks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,StockIn,StockOut,Status")] EmployeeStock employeeStock, int employeeId,
            int ppeAttributeCategoryAttributeValueId, int PpeId)
        {
            var employee = _context.Employees.Find(employeeId);
            if (employee == null)
            {
                return NotFound();
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var newStock = new Stock
                    {
                        ProjectId = employee.ProjectId,
                        Date = employeeStock.Date,
                        //StockIn = employeeStock.StockIn,
                        PpeId = PpeId,
                        StockType = StockType.Normal,
                        StockOut = employeeStock.StockIn,
                    };
                    _context.Stocks.Add(newStock);
                    await _context.SaveChangesAsync();


                   // return Json(newStock);
                    
                    var newStockDetail = new StockDetail
                    {
                        StockId = newStock.Id,
                        PpeAttributeCategoryAttributeValueId = ppeAttributeCategoryAttributeValueId,
                        //StockIn = employeeStock.StockIn,
                        StockOut = employeeStock.StockIn,
                    };
                    _context.StockDetails.Add(newStockDetail);
                    await _context.SaveChangesAsync();
                    
                    //return Json(newStockDetail);
                    
                    var newEmployeeStock = new EmployeeStock
                    {
                        ProjectId = employee.ProjectId,
                        FunctionId = employee.FunctionId,
                        EmployeeId = employee.Id,
                        PpeAttributeCategoryAttributeValueId = ppeAttributeCategoryAttributeValueId,
                        Date = employeeStock.Date,
                        StockIn = employeeStock.StockIn,
                        StockOut = employeeStock.StockOut,
                        Status = employeeStock.Status,
                        Remarks = employeeStock.Remarks
                    };
                    _context.EmployeeStocks.Add(newEmployeeStock);
                    await _context.SaveChangesAsync();
                    
                    await transaction.CommitAsync();
                    return RedirectToAction("Details", "Employees", new { id = employeeId});
                    
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            
            var newStockEmployee = new EmployeeStock
            {
                ProjectId = employee.ProjectId,
                FunctionId = employee.FunctionId,
                EmployeeId = employee.Id,
                PpeAttributeCategoryAttributeValueId = ppeAttributeCategoryAttributeValueId,
                Date = employeeStock.Date,
                StockIn = employeeStock.StockIn,
                StockOut = employeeStock.StockOut,
                Status = employeeStock.Status,
                Remarks = employeeStock.Remarks
            };
            
            //return Json(employeeStock);
            if (ModelState.IsValid)
            {
                _context.EmployeeStocks.Add(newStockEmployee);
                await _context.SaveChangesAsync();
                
                return RedirectToAction("Details", "Employees", new { id = employeeStock.EmployeeId });
                return RedirectToAction(nameof(Index));
            }
            
            //return Json(ModelState.Values.SelectMany(v => v.Errors));
            
            ViewData["Status"] = new SelectList(Enum.GetValues(typeof(StockEmployeeStatus)));
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FirstName", employeeStock.EmployeeId);
            ;//ViewData["VariantValueId"] = new SelectList(_context.VariantValues, "Id", "Id", employeeStock.VariantValueId);
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
            //ViewData["VariantValueId"] = new SelectList(_context.VariantValues, "Id", "Id", employeeStock.VariantValueId);
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
            //ViewData["VariantValueId"] = new SelectList(_context.VariantValues, "Id", "Id", employeeStock.VariantValueId);
            return View(employeeStock);
        }

        // GET: EmployeeStocks/Delete/5
        /*public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.EmployeeStocks == null)
            {
                return NotFound();
            }

            var employeeStock = await _context.EmployeeStocks
                .Include(e => e.Employee)
                //.Include(e => e.VariantValue)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employeeStock == null)
            {
                return NotFound();
            }

            return View(employeeStock);
        }*/

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
        
        public IActionResult GetPpes(int? categoryId, int? projectId)
        {
            if (categoryId == null)
            {
                return NotFound();
            }

            var ppes = _context.Ppes
                .Include(p => p.Category)
                .Include(p => p.PpeAttributeCategoryAttributeValues)
                .ThenInclude(p => p.AttributeValueAttributeCategory)
                .ThenInclude(p => p.AttributeValue)
                .Where(p => p.CategoryId == categoryId)
                .Where(p => p.PpeAttributeCategoryAttributeValues.Any(p => p.StockDetails.Any(s => s.Stock.ProjectId == projectId)))
                .ToList();
            
            var options = "<option value=\"\">Select PPE</option>";
            foreach (var ppe in ppes)
            {
                options += $"<option value=\"{ppe.Id}\">{ppe.Title}</option>";
            }
            
            return Json(options);
        }
        
        // GET: EmployeeStocks/GetAttributesValues/5
        public IActionResult GetAttributesValues(int? ppeId, int? projectId)
        {
            if (ppeId == null)
            {
                return NotFound();
            }

            /*var ppe = _context.Ppes
                .Include(p => p.PpeAttributeCategoryAttributeValues)
                .ThenInclude(p => p.AttributeValueAttributeCategory)
                .ThenInclude(p => p.AttributeValue)
                .ThenInclude(p => p.Value)
                .FirstOrDefault(p => p.Id == ppeId);*/
            
            var ppeAttributeCategoryAttributeValues = _context.PpeAttributeCategoryAttributeValues
                .Include(p => p.AttributeValueAttributeCategory)
                .ThenInclude(p => p.AttributeValue)
                .ThenInclude(p => p.Value)
                .Where(p => p.PpeId == ppeId)
                .Where(p => p.StockDetails.Any(s => s.Stock.ProjectId == projectId))
                .ToList();
            

           // return Json(ppe.PpeAttributeCategoryAttributeValues);
            
            var options = "<option value=\"\">Select Attribute Value</option>";
            foreach (var ppeAttributeCategoryAttributeValue in ppeAttributeCategoryAttributeValues)
            {
                var valueCount = _context.StockDetails
                    .Include(s => s.Stock)
                    .Where(s => s.PpeAttributeCategoryAttributeValueId == ppeAttributeCategoryAttributeValue.Id)
                    .Where(s => s.Stock.ProjectId == projectId)
                    .Sum(s => s.Stock.StockIn - s.Stock.StockOut);
                options += $"<option value=\"{ppeAttributeCategoryAttributeValue.Id}\">{ppeAttributeCategoryAttributeValue.AttributeValueAttributeCategory.AttributeValue.Value.Text} ( {valueCount})</option>";
            }
            
            return Json(options);
        }

        
    }
}
