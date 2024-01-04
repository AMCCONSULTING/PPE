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
    public class MainStocksController : Controller
    {
        private readonly AppDbContext _context;

        public MainStocksController(AppDbContext context)
        {
            _context = context;
        }

        // GET: MainStocks
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.MainStocks
                .Include(m => m.PpeAttributeCategoryAttributeValue)
                .ThenInclude(m => m.Ppe)
                .Include(m => m.PpeAttributeCategoryAttributeValue)
                .ThenInclude(m => m.AttributeValueAttributeCategory)
                .ThenInclude(m => m.AttributeValue)
                .ThenInclude(m => m.Value)
                .GroupBy(m => m.PpeAttributeCategoryAttributeValue.Ppe)
                .Select(m => new MainStock
                {
                    Id = m.Key.Id,
                    PpeAttributeCategoryAttributeValue = m.First().PpeAttributeCategoryAttributeValue,
                    QuantityIn = m.Sum(m => m.QuantityIn),
                    QuantityOut = m.Sum(m => m.QuantityOut),
                    QuantityStock = m.Sum(m => m.QuantityStock)
                });
            return View(await appDbContext.ToListAsync());
        }

        // GET: MainStocks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.MainStocks == null)
            {

                //return Json("Ok");
                return NotFound();
            }
            
            /*return Json( new
            {
                messagef ="ok now",
                id = id,
                mainStock = _context.MainStocks
            });*/

            var mainStock = await _context.MainStocks
                .Include(m => m.PpeAttributeCategoryAttributeValue)
                .FirstOrDefaultAsync(m => m.PpeAttributeCategoryAttributeValueId == id);
            
            var stockDetails = await _context.StokeDetails
                .Include(sd => sd.Stoke)
                .ThenInclude(s => s.Magazinier)
                .Include(sd => sd.Stoke)
                .ThenInclude(s => s.Responsable)
                .Include(sd => sd.Article)
                .ThenInclude(a => a.Ppe)
                .Include(sd => sd.Article)
                .ThenInclude(a => a.AttributeValueAttributeCategory)
                .ThenInclude(avac => avac.AttributeValue)
                .ThenInclude(av => av.Value)
                .Where(sd => sd.Article.Ppe.Id == id)
                .ToListAsync();
            
            if (stockDetails == null)
            {
                //return Json("Ok");
                return NotFound();
            }
            
            var mouvementsDetails = await _context.MouvementDetails
                .Include(md => md.Mouvement)
                .ThenInclude(m => m.Magazinier)
                .Include(md => md.Mouvement)
                .ThenInclude(m => m.Responsable)
                .Include(md => md.Article)
                .ThenInclude(a => a.Ppe)
                .Include(md => md.Article)
                .ThenInclude(a => a.AttributeValueAttributeCategory)
                .ThenInclude(avac => avac.AttributeValue)
                .ThenInclude(av => av.Value)
                .Where(md => md.Article.Ppe.Id == id)
                .ToListAsync();

            /*var viewModels = new CombinedViewModel
            {
                StokeDetails = stockDetails,
                MouvementDetails = mouvementsDetails
            };*/
            
            ViewBag.StokeDetails = mouvementsDetails;
            
            return View(stockDetails);
        }

        // GET: MainStocks/Create
        public IActionResult Create()
        {
            ViewData["PpeAttributeCategoryAttributeValueId"] = new SelectList(_context.PpeAttributeCategoryAttributeValues, "Id", "Id");
            return View();
        }

        // POST: MainStocks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PpeAttributeCategoryAttributeValueId,QuantityIn,QuantityOut,QuantityStock")] MainStock mainStock)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mainStock);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PpeAttributeCategoryAttributeValueId"] = new SelectList(_context.PpeAttributeCategoryAttributeValues, "Id", "Id", mainStock.PpeAttributeCategoryAttributeValueId);
            return View(mainStock);
        }

        // GET: MainStocks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.MainStocks == null)
            {
                return NotFound();
            }

            var mainStock = await _context.MainStocks.FindAsync(id);
            if (mainStock == null)
            {
                return NotFound();
            }
            ViewData["PpeAttributeCategoryAttributeValueId"] = new SelectList(_context.PpeAttributeCategoryAttributeValues, "Id", "Id", mainStock.PpeAttributeCategoryAttributeValueId);
            return View(mainStock);
        }

        // POST: MainStocks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PpeAttributeCategoryAttributeValueId,QuantityIn,QuantityOut,QuantityStock")] MainStock mainStock)
        {
            if (id != mainStock.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mainStock);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MainStockExists(mainStock.Id))
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
            ViewData["PpeAttributeCategoryAttributeValueId"] = new SelectList(_context.PpeAttributeCategoryAttributeValues, "Id", "Id", mainStock.PpeAttributeCategoryAttributeValueId);
            return View(mainStock);
        }

        // GET: MainStocks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.MainStocks == null)
            {
                return NotFound();
            }

            var mainStock = await _context.MainStocks
                .Include(m => m.PpeAttributeCategoryAttributeValue)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mainStock == null)
            {
                return NotFound();
            }

            return View(mainStock);
        }

        // POST: MainStocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.MainStocks == null)
            {
                return Problem("Entity set 'AppDbContext.MainStock'  is null.");
            }
            var mainStock = await _context.MainStocks.FindAsync(id);
            if (mainStock != null)
            {
                _context.MainStocks.Remove(mainStock);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MainStockExists(int id)
        {
          return (_context.MainStocks?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
