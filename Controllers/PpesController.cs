using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataTables.AspNetCore.Mvc.Binder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PPE.Data;
using PPE.Models;

namespace PPE.Controllers
{
    public class PpesController : Controller
    {
        private readonly AppDbContext _context;

        public PpesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Ppes
        public async Task<IActionResult> Index()
        {
            //var appDbContext = _context.Ppes.Include(p => p.Category);
            ViewBag.Categories = _context.Categories
            .Select(c => new SelectListItem{
                Text = c.Title,
                Value = c.Id.ToString()
            });
            return View();
        }

        // GET: Ppes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Ppes == null)
            {
                return NotFound();
            }

            var ppe = await _context.Ppes
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ppe == null)
            {
                return NotFound();
            }

            return View(ppe);
        }

        // GET: Ppes/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, 
                "Id", 
                "Title");
            return View();
        }
        
        // GET: Ppes/api
        [HttpGet()]
        [Route("api/ppe")]
        public IActionResult Get([DataTablesRequest] DataTablesRequest dataRequest)
        {
            IEnumerable<Ppe> ppe = _context.Ppes.Include(p => p.Category);
            
            int recordsTotal = ppe.Count();
            int recordsFilterd = recordsTotal;

            /*if (filters.Category > 0)
            {
                ppe = ppe.Where(e => e.CategoryId == filters.Category);
                recordsFilterd = ppe.Count();
            }*/
            
            if (!string.IsNullOrEmpty(dataRequest.Search?.Value))
            {
                ppe = ppe.Where(e => e.Title.Contains(dataRequest.Search.Value, StringComparison.InvariantCultureIgnoreCase));
                recordsFilterd = ppe.Count();
            }
            
            ppe = ppe.Skip(dataRequest.Start).Take(dataRequest.Length);
            var deleteUrl = "Ppes/DeletePpe";
            
            return Json(ppe
                .Select(e => new
                {
                    Id = e.Id,
                    Title = e.Title,
                    Description = e.Description,
                    Category = e.Category.Title,
                    Threshold = e.Threshold,
                    Actions = $"<div class='btn-group'>" +
                              $"<a class='btn btn-primary btn-sm' href='/Ppes/Edit/{e.Id}'>" +
                              $"<i class='la la-edit'></i>"+
                              $"</a>"+
                              $"<a class='btn btn-danger btn-sm' onclick='confirmDelete(\"{deleteUrl}\", {e.Id})'>" +
                              $"<i class='la la-trash text-white'></i>"+
                              $"</a>"+
                              $"</div>"
                })
                .ToDataTablesResponse(dataRequest, recordsTotal, recordsFilterd));
            
        }

        // POST: Ppes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,CategoryId,Threshold")] Ppe ppe)
        {
            if (ModelState.IsValid)
            {
                await _context.AddAsync(ppe);
                await _context.SaveChangesAsync();

                var categoryVariant = _context.Variants.FirstOrDefault(v => v.CategoryId == ppe.CategoryId);
                // add the ppe to the Variant table with category id as variant id of the same category

                if (categoryVariant == null)
                {
                    var variant = new Variant
                                    {
                                        Title = categoryVariant!.Title,
                                        PpeId = ppe.Id,
                                        CategoryId = ppe.CategoryId
                                    };
                                    
                                     await _context.Variants.AddAsync(variant);
                                     await _context.SaveChangesAsync();
                                     
                    
                }
                
                
                /*
                 *    // add the ppe to the VariantValue table with variant id as variant id of the same category
                var categoryVariantValues = _context.VariantValues
                    .Where(v => v.VariantId == categoryVariant.Id)
                    .ToList();
                
                var variantValues = categoryVariantValues.Select(v => new VariantValue
                {
                    ValueId = v.ValueId,
                    VariantId = variant.Id
                }).ToList();

                // return Json(variantValues);
                
                await _context.VariantValues.AddRangeAsync(variantValues);
                 */
                
                await _context.SaveChangesAsync();
                
               
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", 
                "Title", ppe.CategoryId);
            return View(ppe);
        }

        // GET: Ppes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Ppes == null)
            {
                return NotFound();
            }

            var ppe = await _context.Ppes.FindAsync(id);
            if (ppe == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", 
                "Title", ppe.CategoryId);
            return View(ppe);
        }

        // POST: Ppes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,CategoryId,Threshold")] Ppe ppe)
        {
            if (id != ppe.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ppe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PpeExists(ppe.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", ppe.CategoryId);
            return View(ppe);
        }

        // GET: Ppes/Delete/5
        /*public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Ppes == null)
            {
                return NotFound();
            }

            var ppe = await _context.Ppes
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ppe == null)
            {
                return NotFound();
            }

            return View(ppe);
        }

        // POST: Ppes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Ppes == null)
            {
                return Problem("Entity set 'AppDbContext.Ppes'  is null.");
            }
            var ppe = await _context.Ppes.FindAsync(id);
            if (ppe != null)
            {
                _context.Ppes.Remove(ppe);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        */

        // POST: Ppes/Delete/5
        [HttpPost]
        public IActionResult DeletePpe(int id)
        {
            if (_context.Ppes == null)
            {
                return Problem("Entity set 'AppDbContext.Ppes'  is null.");
            }
            var ppe = _context.Ppes.Find(id);
            if (ppe != null)
            {
                _context.Ppes.Remove(ppe);
            }
            
            _context.SaveChanges();
            return Json(new { success = true, message = "Delete successful" });
        }
        
        private bool PpeExists(int id)
        {
          return (_context.Ppes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
