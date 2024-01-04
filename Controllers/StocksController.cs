using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PPE.Data;
using PPE.Data.Enums;
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
            
            var appDbContext = _context.Stocks
                .Where(s => s.Project == null && s.StockType == StockType.Normal)
                .Include(s => s.Ppe)
                .ThenInclude(s => s.Category)
                .Include(s => s.StockDetails)
                .ThenInclude(s => s.PpeAttributeCategoryAttributeValue)
                .ThenInclude(s => s.AttributeValueAttributeCategory)
                .ThenInclude(s => s.AttributeValue)
                .ThenInclude(s => s.Value)
                .Include(s => s.StockDetails)
                .ThenInclude(s => s.PpeAttributeCategoryAttributeValue)
                .ThenInclude(s => s.AttributeValueAttributeCategory)
                .ThenInclude(s => s.AttributeCategory)
                .ThenInclude(s => s.Attribute)
                .Include(s => s.StockDetails)
                .ThenInclude(s => s.PpeAttributeCategoryAttributeValue)
                .ThenInclude(s => s.Ppe);
            
            // stock of each ppe and sum of stock in and stock out
            var stockOfPpe = appDbContext
                .GroupBy(s => s.Ppe)
                .Select(s => new 
                { 
                    Ppe = s.Key,
                    StockIn = s.Sum(s => s.StockIn),
                    StockOut = s.Sum(s => s.StockOut),
                    CurrentStock = s.Sum(s => s.StockIn) - s.Sum(s => s.StockOut),
                    Threshold = s.Key.Threshold,
                });
            
            //return Json(stockOfPpe);
            
            ViewBag.StockOfPpe = stockOfPpe;
            
            return View(await appDbContext.ToListAsync());
        }

        // GET: Stocks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Ppes == null)
            {
                return NotFound();
            }
            
            var ppe = await _context.Ppes
                .Where(p => p.Id == id)
                .Include(p => p.Category)
                .FirstOrDefaultAsync();

            var stockDetails = _context.StockDetails
                .Where(s => s.Stock.PpeId == id && s.Stock.Project == null && s.Stock.StockType == StockType.Normal)
                .Include(s => s.PpeAttributeCategoryAttributeValue)
                .ThenInclude(s => s.AttributeValueAttributeCategory)
                .ThenInclude(s => s.AttributeValue)
                .ThenInclude(s => s.Value)
                .Include(s => s.PpeAttributeCategoryAttributeValue)
                .ThenInclude(s => s.AttributeValueAttributeCategory)
                .ThenInclude(s => s.AttributeCategory)
                .ThenInclude(s => s.Attribute)
                .Include(s => s.PpeAttributeCategoryAttributeValue)
                .ThenInclude(s => s.Ppe)
                .GroupBy(s => s.PpeAttributeCategoryAttributeValue.AttributeValueAttributeCategory.AttributeValue.Value.Text);
            
            ViewBag.Ppe = ppe;
            
            // sum of ppes stock in and stock out of each attribute value of ppe
            var stockOfPpe = stockDetails
                .Select(s => new 
                { 
                    Value = s.Key,
                    StockIn = s.Sum(s => s.StockIn),
                    StockOut = s.Sum(s => s.StockOut),
                    CurrentStock = s.Sum(s => s.StockIn) - s.Sum(s => s.StockOut),
                });
          
            ViewBag.StockDetailed = stockOfPpe;
           // return Json(ViewBag.StockDetailed);
            return View();
        }

        // GET: Stocks/Create
        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Title");
            
            return View();
        }
        
        // GET: Stocks/GetPpeOfCategory/5
        public async Task<IActionResult> GetPpeOfCategory(int? id)
        {
            if (id == null || _context.Ppes == null)
            {
                return NotFound();
            }

            var ppe = await _context.Ppes
                .Where(p => p.CategoryId == id)
                .Select(p => new SelectListItem
                {
                    Text = p.Title,
                    Value = p.Id.ToString(),
                })
                .ToListAsync();
            
            // create select list for ppe with option to select html element
            var options = "<option value=''>-- Select an option --</option>";
            foreach (var p in ppe)
            {
                options += $"<option " + $"value='{p.Value}'>{p.Text}</option>";
            }
            
            return Json(options);
        }
        
        // GET: Stocks/GetPpeAttributeCategoryAttributeValueOfPpe/5
        public async Task<IActionResult> GetPpeAttributeCategoryAttributeValueOfPpe(int? id)
        {
            if (id == null || _context.PpeAttributeCategoryAttributeValues == null)
            {
                return NotFound();
            }

            var ppeAttributeCategoryAttributeValue = await _context.PpeAttributeCategoryAttributeValues
                .Where(p => p.PpeId == id)
                .Include(p => p.StockDetails)
                .Select(p => new SelectListItem
                {
                    Text = p.AttributeValueAttributeCategory.AttributeValue.Value.Text,
                    Value = p.Id.ToString(),
                })
                .ToListAsync();
            
            // create select list for ppe with option to select html element
            var options = "<option value=''>-- Select an option --</option>";
            foreach (var p in ppeAttributeCategoryAttributeValue)
            {
                options += $"<option " + $"value='{p.Value}'>{p.Text}</option>";
            }
            
            return Json(options);
        }
        
        
        public async Task<IActionResult> GetAttributeValueOfPpe(int? id)
        {
            if (id == null || _context.PpeAttributeCategoryAttributeValues == null)
            {
                return NotFound();
            }
            
            var ppeAttributeValueInStockDetails = 
                await _context.StockDetails
                    .Where(s => s.Stock.PpeId == id &&
                                s.Stock.Project == null && 
                                s.Stock.StockType == StockType.Normal)
                    .Include(s => s.PpeAttributeCategoryAttributeValue)
                    .ThenInclude(s => s.AttributeValueAttributeCategory)
                    .ThenInclude(s => s.AttributeValue)
                    .ThenInclude(s => s.Value)
                    .GroupBy(s => s.PpeAttributeCategoryAttributeValue.AttributeValueAttributeCategory.AttributeValue.Value.Text)
                    .Select(s => s.FirstOrDefault())
               
                    .ToListAsync();

            //return Json(ppeAttributeValueInStockDetails);
            
            var ppeAttributeCategoryAttributeValue = await _context.PpeAttributeCategoryAttributeValues
                .Where(p => p.PpeId == id)
                .Include(p => p.StockDetails)
                
                .Select(p => new SelectListItem
                {
                    Text = p.AttributeValueAttributeCategory.AttributeValue.Value.Text,
                    Value = p.Id.ToString(),
                })
                .ToListAsync();
            
            // create select list for ppe with option to select html element
            var options = "<option value=''>-- Select an option --</option>";
            foreach (var p in ppeAttributeValueInStockDetails)
            {
                options += $"<option " + $"value='{p.PpeAttributeCategoryAttributeValueId}'>{p.PpeAttributeCategoryAttributeValue.AttributeValueAttributeCategory.AttributeValue.Value.Text}</option>";
            }
            
            return Json(options);
        }
        
        // POST: Stocks/SaveStocks
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Date,ProjectId,StockIn,StockOut,PpeId,NatureStock,StockType")] Stock stock,
            string attributeValueId,
            List<int> stockIn, List<int> ppeVariantId, List<int> ppeIds)
        {
            /*return Json(
                new
                {
                    ppeIds,
                    stockIn,
                    ppeVariantId,
                });*/
            if (ModelState.IsValid)
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        /*_context.Add(stock);
                        await _context.SaveChangesAsync();*/

                        if (stock.ProjectId == null)
                        {
                            for (int i = 0; i < ppeVariantId.Count; i++)
                            {
                                var newStockIn = new Stock
                                {
                                    Date = stock.Date,
                                    StockIn = stockIn[i],
                                    StockOut = 0,
                                    PpeId = ppeIds[i],
                                    StockType = StockType.Normal,
                                    StockNature = StockNature.Administration,
                                };
                                _context.Add(newStockIn);
                                await _context.SaveChangesAsync();
                                
                                var newStockDetails = new StockDetail
                                {
                                    StockId = newStockIn.Id,
                                    PpeAttributeCategoryAttributeValueId = ppeVariantId[i],
                                    StockIn = stockIn[i],
                                    StockOut = 0,
                                };
                                _context.Add(newStockDetails);
                               await _context.SaveChangesAsync();
                            }
                            await _context.SaveChangesAsync();
                            await transaction.CommitAsync();
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            /*return Json(new
                            {
                                ppeIds,
                                stockIn,
                                ppeVariantId,
                            });*/
                            for (int i = 0; i < ppeVariantId.Count; i++)
                            {
                                var newStockOutP = new Stock
                                {
                                    Date = stock.Date,
                                    StockIn = 0,
                                    StockOut = stockIn[i],
                                    PpeId = ppeIds[i],
                                    StockType = StockType.Normal,
                                    StockNature = StockNature.Administration,
                                    //ProjectId = stock.ProjectId,
                                };
                                _context.Stocks.Add(newStockOutP);
                                await _context.SaveChangesAsync();
                                
                                var newStockIn = new Stock
                                {
                                    Date = stock.Date,
                                    StockIn = stockIn[i],
                                    StockOut = 0,
                                    PpeId = ppeIds[i],
                                    StockType = StockType.Normal,
                                    StockNature = StockNature.Project,
                                    ProjectId = stock.ProjectId,
                                };
                                _context.Add(newStockIn);
                                await _context.SaveChangesAsync();

                                var newStockDetailsOut = new StockDetail
                                {
                                    StockId = newStockOutP.Id,
                                    PpeAttributeCategoryAttributeValueId = ppeVariantId[i],
                                    StockIn = 0,
                                    StockOut = stockIn[i],
                                };
                                _context.Add(newStockDetailsOut);
                                
                                var newStockDetails = new StockDetail
                                {
                                    StockId = newStockIn.Id,
                                    PpeAttributeCategoryAttributeValueId = ppeVariantId[i],
                                    StockIn = stockIn[i],
                                    StockOut = 0,
                                };
                                _context.Add(newStockDetails);
                            }
                            
                            await _context.SaveChangesAsync();
                            
                            await transaction.CommitAsync();
                            return RedirectToAction(nameof(Index));
                        }
                    } catch (DbUpdateException ex)
                    {
                        await transaction.RollbackAsync();
                        return Json(GetFullErrorMessage(ex));
                    }
                }
            }
            
            //return Json(ModelState.Values.SelectMany(v => v.Errors));
            
            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Title");
            ViewBag.ProjectId = new SelectList(_context.Projects, "Id", "Title");
            return View(stock);
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

            
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Prefix", stock.ProjectId);
            return View(stock);
        }

        // GET: Stocks/Delete/5
        /*public async Task<IActionResult> Delete(int? id)
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
        }*/

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

        public async Task<IActionResult> StockReturn()
        {
            
            var stockReturn = _context.EmployeeStocks
                .Where(s => s.Status == StockEmployeeStatus.Returned || s.Status == StockEmployeeStatus.Damaged)
                .Include(s => s.Employee)
                .Include(s => s.PpeAttributeCategoryAttributeValue)
                .ThenInclude(s => s.AttributeValueAttributeCategory)
                .ThenInclude(s => s.AttributeValue)
                .ThenInclude(s => s.Value)
                .Include(s => s.PpeAttributeCategoryAttributeValue)
                .ThenInclude(s => s.AttributeValueAttributeCategory)
                .ThenInclude(s => s.AttributeCategory)
                .ThenInclude(s => s.Attribute)
                .Include(s => s.PpeAttributeCategoryAttributeValue)
                .ThenInclude(s => s.Ppe);

            //return Json(stockReturn);
            // stock of each ppe and sum of stock in and stock out
            var stockOfPpe = stockReturn
                .Include(s => s.PpeAttributeCategoryAttributeValue)
                .ThenInclude(s => s.Ppe)
                .ThenInclude(s => s.Category)
                .GroupBy(s => s.PpeAttributeCategoryAttributeValue.Ppe)
                .Select(s => new 
                { 
                    Ppe = s.Key.Title,
                    Category = s.Key.Category.Title,
                    Status = s.FirstOrDefault().Status,
                    StockIn = s.Sum(s => s.StockIn),
                    StockOut = s.Sum(s => s.StockOut),
                    CurrentStock = s.Sum(s => s.StockIn) - s.Sum(s => s.StockOut),
                });
            //return Json(stockOfPpe);
            /*var appDbContext = _context.Stocks
                .Where(s => s.Project == null && s.StockType == StockType.Return)
                .Include(s => s.Ppe)
                .ThenInclude(s => s.Category)
                .Include(s => s.StockDetails)
                .ThenInclude(s => s.PpeAttributeCategoryAttributeValue)
                .ThenInclude(s => s.AttributeValueAttributeCategory)
                .ThenInclude(s => s.AttributeValue)
                .ThenInclude(s => s.Value)
                .Include(s => s.StockDetails)
                .ThenInclude(s => s.PpeAttributeCategoryAttributeValue)
                .ThenInclude(s => s.AttributeValueAttributeCategory)
                .ThenInclude(s => s.AttributeCategory)
                .ThenInclude(s => s.Attribute)
                .Include(s => s.StockDetails)
                .ThenInclude(s => s.PpeAttributeCategoryAttributeValue)
                .ThenInclude(s => s.Ppe);*/
            
            // stock of each ppe and sum of stock in and stock out
            /*var stockOfPpe = appDbContext
                .GroupBy(s => s.Ppe)
                .Select(s => new 
                { 
                    Ppe = s.Key,
                    StockIn = s.Sum(s => s.StockIn),
                    StockOut = s.Sum(s => s.StockOut),
                    CurrentStock = s.Sum(s => s.StockIn) - s.Sum(s => s.StockOut),
                    Threshold = s.Key.Threshold,
                });*/
            
            ViewBag.StockOfPpe = stockOfPpe;
            
            return View();
        }

        public async Task<IActionResult> StockLost()
        {
            var appDbContext = _context.EmployeeStocks
                .Where(s => s.Status == StockEmployeeStatus.Lost)
                .Include(s => s.Employee)
                .Include(s => s.PpeAttributeCategoryAttributeValue)
                .ThenInclude(s => s.AttributeValueAttributeCategory)
                .ThenInclude(s => s.AttributeValue)
                .ThenInclude(s => s.Value)
                .Include(s => s.PpeAttributeCategoryAttributeValue)
                .ThenInclude(s => s.AttributeValueAttributeCategory)
                .ThenInclude(s => s.AttributeCategory)
                .ThenInclude(s => s.Attribute)
                .Include(s => s.PpeAttributeCategoryAttributeValue)
                .ThenInclude(s => s.Ppe);
            
            
            // stock of each ppe and sum of stock in and stock out
            var stockOfPpe = appDbContext
                .GroupBy(s => s.PpeAttributeCategoryAttributeValue.Ppe)
                .Select(s => new 
                { 
                    Ppe = s.Key,
                    Category = s.Key.Category.Title,
                    StockIn = s.Sum(s => s.StockIn),
                    StockOut = s.Sum(s => s.StockOut),
                    CurrentStock = s.Sum(s => s.StockIn) - s.Sum(s => s.StockOut),
                    Threshold = s.Key.Threshold,
                });
            
            //return Json(stockOfPpe);
            
            ViewBag.StockOfPpe = stockOfPpe;
            
            return View();
        }

        // GET: Stocks/CreateMovement
        public IActionResult CreateMovement()
        {
            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Title");
            ViewBag.ProjectId = new SelectList(_context.Projects, "Id", "Title");
            return View();
        }
        
        private string GetFullErrorMessage(DbUpdateException ex)
        {
            var messages = new List<string>();
            Exception currentException = ex;

            while (currentException != null)
            {
                messages.Add(currentException.Message);
                currentException = currentException.InnerException;
            }

            return string.Join(" ", messages);
        }
        
    }
}
