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

            ViewBag.PpeAttributeCategoryAttributeValues = _context.PpeAttributeCategoryAttributeValues
                .Where(pacav => pacav.PpeId == id)
                .Include(pacav => pacav.AttributeValueAttributeCategory)
                .ThenInclude(avac => avac.AttributeValue)
                .ThenInclude(av => av.Value)
                .Include(pacav => pacav.AttributeValueAttributeCategory)
                .ThenInclude(avac => avac.AttributeValue)
                .ThenInclude(ac => ac.Attribute)
                .Select(pacav => new SelectListItem()
                {
                    Text = pacav.AttributeValueAttributeCategory.AttributeValue.Value.Text,
                    Value = pacav.AttributeValueAttributeCategory.AttributeValue.Value.Id.ToString(),
                    Group = new SelectListGroup()
                    {
                        Name = pacav.AttributeValueAttributeCategory.AttributeValue.Attribute.Title
                    }
                });

            //return Json(ViewBag.PpeAttributeCategoryAttributeValues);
            
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
            //IEnumerable<Ppe> ppe = _context.Ppes.Include(p => p.Category);
            IQueryable<Ppe> ppe = _context.Ppes.Include(p => p.Category);
            
            int recordsTotal = ppe.Count();
            int recordsFilterd = recordsTotal;

            var filters = new Dictionary<string, string>();
            foreach(var key in Request.Query.Keys)
            {
                if (key.StartsWith("filters"))
                {
                    filters[key.Substring("filters".Length)] = Request.Query[key];
                    
                    if (key == "filters[category]" && !string.IsNullOrEmpty(Request.Query[key]))
                    {
                        ppe = ppe.Where(e => e.CategoryId == int.Parse(Request.Query[key]));
                        recordsFilterd = ppe.Count();
                    }
                    if (key == "filters[title]" && !string.IsNullOrEmpty(Request.Query[key]))
                    {
                        ppe = ppe.Where(e => e.Title.Contains(Request.Query[key].ToString()));
                        recordsFilterd = ppe.Count();
                    }
                }
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
                await using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    await _context.AddAsync(ppe);
                    await _context.SaveChangesAsync();
                    
                    var category = _context.Categories.FirstOrDefault(c => c.Id == ppe.CategoryId);
                    if (category == null)
                    {
                        return NotFound();
                    }
                    
                   var attributeValueAttributeCategoryId = _context.AttrValueAttrCategories
                          .Where(avac => avac.AttributeCategory.CategoryId == category.Id)
                          .Select(avac => avac.Id)
                          .ToList();
                   
                   var ppeAttributeCategoryAttributeValue = 
                       attributeValueAttributeCategoryId.Select(avac => new PpeAttributeCategoryAttributeValue
                   {
                       AttributeValueAttributeCategoryId = avac,
                          PpeId = ppe.Id
                   } ).ToList();
                   
                   _context.PpeAttributeCategoryAttributeValues.AddRange(ppeAttributeCategoryAttributeValue);
                   await _context.SaveChangesAsync();
                   await transaction.CommitAsync();
                   
                   return RedirectToAction(nameof(Index));
                   
                }
                catch (DbUpdateException e)
                {
                    await transaction.RollbackAsync();
                    return Problem(e.Message);
                }
            }

            /*if (ModelState.IsValid)
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
                 #1#

                await _context.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id",
                "Title", ppe.CategoryId);*/
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
