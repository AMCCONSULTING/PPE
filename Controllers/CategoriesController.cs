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
    public class CategoriesController : Controller
    {
        private readonly AppDbContext _context;

        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
              return _context.Categories != null ? 
                          View(await _context.Categories
                                .Include(c => c.Ppes)
                              .ToListAsync()) :
                          Problem("Entity set 'AppDbContext.Categories'  is null.");
        }
        
        [HttpGet()]
        [Route("api/value")]
        public IActionResult Get([DataTablesRequest] DataTablesRequest dataRequest)
        {
            IEnumerable<Category> categories = _context.Categories.Include(c => c.Ppes);
            int recordsTotal = categories.Count();
            int recordsFilterd = recordsTotal;

            if (!string.IsNullOrEmpty(dataRequest.Search?.Value))
            {
                categories = categories
                    .Where(e => e.Title.Contains(dataRequest.Search.Value));
                recordsFilterd = categories.Count();
            }
            categories = categories.Skip(dataRequest.Start).Take(dataRequest.Length);
            var deleteUrl = "Categories/DeleteCategory";
            return Json(categories
                .Select(e => new
                {
                    Id = e.Id,
                    Title = e.Title,
                    PpeCount = e.Ppes!.Count,
                    Description = e.Description,
                    Actions = $"<div class='btn-group'>" +
                              $"<a class='btn btn-primary btn-sm' href='/Categories/Edit/{e.Id}'>" +
                              $"<i class='la la-edit'></i>"+
                              $"</a>"+
                              $"<a class='btn btn-danger btn-sm' onclick='confirmDelete(\"{deleteUrl}\", {e.Id})' id='12'>" +
                              $"<i class='la la-trash text-white'></i>"+
                              $"</a>"+
                              $"</div>"
                              
                })
                .ToDataTablesResponse(dataRequest, recordsTotal, recordsFilterd));
        }
        
        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.Ppes)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {

            ViewBag.Attributes = _context.Attributes.
                Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Title
                }).ToList();
            
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description")] Category category, List<string> attributes)
        {
            
           //return Json(attributes);
            
            if (ModelState.IsValid)
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    _context.Add(category);
                    await _context.SaveChangesAsync();
                    if (attributes != null)
                    {
                        foreach (var attributeIdString in attributes)
                        {
                            if (int.TryParse(attributeIdString, out var attributeId))
                            {
                                
                                // get all valueIds from attributeValues
                                var attributeValues = _context.AttributeValues.Where(av => av.AttributeId == attributeId);
                                var valueIds = attributeValues.Select(av => av.ValueId).ToList();

                                var attributeCategory = new AttributeCategory
                                {
                                    AttributeId = attributeId,
                                    CategoryId = category.Id,
                                    // Set other properties as needed
                                };

                                await _context.AttributeCategories.AddAsync(attributeCategory);
                                await _context.SaveChangesAsync();
                                
                                foreach (var valueId in valueIds)
                                {
                                    // if attributeValue is already in the database, don't add it again
                                    if (_context.AttributeValues.Any(av => av.AttributeId == attributeId && av.ValueId == valueId))
                                    {
                                        // get the the record from the database and add it to the AttributeValueAttributeCategory table
                                        var attributeValue = _context.AttributeValues.FirstOrDefault(av => av.AttributeId == attributeId && av.ValueId == valueId);
                                        _context.AttrValueAttrCategories.Add(new AttributeValueAttributeCategory
                                        {
                                            AttributeCategoryId = attributeCategory.Id,
                                            AttributeValueId = attributeValue.Id,
                                            // Set other properties as needed
                                        });
                                    } else
                                    {
                                        // add the attributeValue to the database and then add it to the AttributeValueAttributeCategory table
                                        var attributeValue = new AttributeValue
                                        {
                                            AttributeId = attributeId,
                                            ValueId = valueId,
                                            // Set other properties as needed
                                        };
                                        await _context.AttributeValues.AddAsync(attributeValue);
                                        await _context.SaveChangesAsync();
                                        _context.AttrValueAttrCategories.Add(new AttributeValueAttributeCategory
                                        {
                                            AttributeCategoryId = attributeCategory.Id,
                                            AttributeValueId = attributeValue.Id,
                                            // Set other properties as needed
                                        });
                                    }
                                }
                            }
                        }

                        await _context.SaveChangesAsync();
                    }
                    
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException e)
                {
                    await transaction.RollbackAsync();
                    ModelState.AddModelError("", GetFullErrorMessage(e));
                }

                /*_context.Add(category);
                await _context.SaveChangesAsync();
                if (attributes != null)
                {
                    var attrCategories = attributes.Select(a => new AttributeCategory
                    {
                        AttributeId = int.Parse(a),
                        CategoryId = category.Id
                    });
                   return Json(attrCategories);
                   //attrCategories.ToList().ForEach(ac => _context.AttrValueAttrCategories.Add(ac));
                   
                }
                
                
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));*/
            }
            ViewBag.Attributes = _context.Attributes.
                Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Title
                }).ToList();
            return View(category);
        }
        /*{
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }*/

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            return View(category);
        }

        /*// Delete: Categories/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'AppDbContext.Categories'  is null.");
            }
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }
            
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Delete successful" });
        }
        
        // DELETE: api

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'AppDbContext.Categories'  is null.");
            }
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }*/
        
        // delete using ajax
        [HttpPost]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'AppDbContext.Categories'  is null.");
            }
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }
            
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Delete successful" });
        }

        private bool CategoryExists(int id)
        {
          return (_context.Categories?.Any(e => e.Id == id)).GetValueOrDefault();
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

