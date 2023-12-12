using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PPE.Data;
using PPE.Models;
using Attribute = PPE.Models.Attribute;

namespace PPE.Controllers
{
    public class AttributesController : Controller
    {
        private readonly AppDbContext _context;

        public AttributesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Attributes
        public async Task<IActionResult> Index()
        {
              return _context.Attributes != null ? 
                          View(await _context.Attributes
                              .Include(a => a.AttributeValues)
                              .ThenInclude(av => av.Value)
                              .ToListAsync()) :
                          Problem("Entity set 'AppDbContext.Attributes'  is null.");
        }

        // GET: Attributes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Attributes == null)
            {
                return NotFound();
            }

            var attribute = await _context.Attributes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (attribute == null)
            {
                return NotFound();
            }

            return View(attribute);
        }

        // GET: Attributes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Attributes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        /*public async Task<IActionResult> Create([Bind("Id,Title")] Attribute attribute, List<string> values)
        {
            if (ModelState.IsValid)
            {
                _context.Add(attribute);
                await _context.SaveChangesAsync();
                
                // Step 1: Save values to Values table
                List<AttributeValue> attributeValues = values.Select(value => new AttributeValue
                {
                    AttributeId = attribute.Id,
                    Value = new Value
                    {
                        Text = value
                    }
                }).ToList();
                
                _context.AttributeValues.AddRange(attributeValues);
                await _context.SaveChangesAsync();
                
                // Step 2: Retrieve the generated IDs of the saved values
                var savedValueIds = attributeValues
                    .Select(value => value.Id).ToList();
                
                // Step 3: Associate the values with the Attribute

                attribute.AttributeValues = savedValueIds.Select(valueId => new AttributeValue
                {
                    AttributeId = attribute.Id,
                    ValueId = valueId
                }).ToList();
                
                _context.Update(attribute);
                await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
            
            }
            return View(attribute);
        }
        */
        
        /*
        public async Task<IActionResult> Create([Bind("Id,Title")] Attribute attribute, List<string> values)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Step 1: Save the attribute
                    _context.Add(attribute);
                    await _context.SaveChangesAsync();

                    // Step 2: Save values to Values table
                    List<AttributeValue> attributeValues = values.Select(value => new AttributeValue
                    {
                        AttributeId = attribute.Id,
                        Value = new Value
                        {
                            Text = value
                        }
                    }).ToList();
                    _context.AttributeValues.AddRange(attributeValues);
                    await _context.SaveChangesAsync();
                    
                    var savedValueIds = attributeValues
                        .Select(value => value.Id)
                        .ToList();
                    
                    // Step 3: Associate the values with the Attribute
                    attribute.AttributeValues = savedValueIds.Select(valueId => new AttributeValue
                    {
                        AttributeId = attribute.Id,
                        ValueId = valueId
                    }).ToList();
                    
                    _context.Update(attribute);
                    
                    await _context.SaveChangesAsync();

                    // Step 3: Retrieve the generated IDs of the saved values
                    /*var savedValueIds = attributeValues
                        .Select(value => value.Id)
                        .ToList();

                    // Step 4: Associate the values with the Attribute
                    attribute.AttributeValues = savedValueIds.Select(valueId => new AttributeValue
                    {
                        AttributeId = attribute.Id,
                        ValueId = valueId
                    }).ToList();

                    _context.Update(attribute);
                    await _context.SaveChangesAsync();#1#

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    // Handle the exception, log, or return an error message
                    // For example: 
                    ModelState.AddModelError("", GetFullErrorMessage(ex));
                    return View(attribute);
                }
            }
            return View(attribute);
        }
        */

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title")] Attribute attribute, List<string> values)
        {
            if (ModelState.IsValid)
            {
                await using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // Step 1: Save the attribute
                        _context.Add(attribute);
                        await _context.SaveChangesAsync();

                        // Step 2: Save values to Values table
                        List<AttributeValue> attributeValues = values.Select(value => new AttributeValue
                        {
                            AttributeId = attribute.Id,
                            Value = new Value
                            {
                                Text = value
                            }
                        }).ToList();

                        _context.AttributeValues.AddRange(attributeValues);
                        await _context.SaveChangesAsync();

                        // Commit the transaction
                        await transaction.CommitAsync();

                        return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateException ex)
                    {
                        // Rollback the transaction
                        await transaction.RollbackAsync();

                        // Log or handle the error
                        var errorMessage = GetFullErrorMessage(ex);
                        ModelState.AddModelError("", errorMessage);
                        return View(attribute);
                    }
                }
            }
            return View(attribute);
        }
        
        // GET: Attributes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Attributes == null)
            {
                return NotFound();
            }

            var attribute = await _context.Attributes.FindAsync(id);
            if (attribute == null)
            {
                return NotFound();
            }
            
            ViewBag.Values = _context.AttributeValues
                .Where(value => value.AttributeId == attribute.Id)
                .Select(value => new SelectListItem
                {
                    Text = value.Value.Text,
                    Value = value.Value.Id.ToString()
                })
                .ToList();
            
            return View(attribute);
        }

        // POST: Attributes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title")] Attribute attribute, List<string> values)
        {
            if (id != attribute.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(attribute);
                    await _context.SaveChangesAsync();
                    
                    // Step 1: Save values to Values table
                    List<AttributeValue> attributeValues = values.Select(value => new AttributeValue
                    {
                        AttributeId = attribute.Id,
                        Value = new Value
                        {
                            Text = value
                        }
                    }).ToList();
                    
                    _context.AttributeValues.AddRange(attributeValues);
                    await _context.SaveChangesAsync();
                    
                    // Step 2: Retrieve the generated IDs of the saved values
                    var savedValueIds = attributeValues
                        .Select(value => value.Id).ToList();
                    
                    // Step 3: Associate the values with the Attribute

                    attribute.AttributeValues = savedValueIds.Select(valueId => new AttributeValue
                    {
                        AttributeId = attribute.Id,
                        ValueId = valueId
                    }).ToList();
                    
                    _context.Update(attribute);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttributeExists(attribute.Id))
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
            return View(attribute);
        }
        */

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title")] Attribute attribute, List<string> values)
        {
            if (id != attribute.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Step 1: Update the attribute
                    _context.Update(attribute);
                    await _context.SaveChangesAsync();

                    // Step 2: Get existing attribute values
                    var existingAttributeValues = _context.AttributeValues
                        .Where(av => av.AttributeId == attribute.Id)
                        .ToList();

                    // Step 3: Update existing attribute values with new values
                    for (int i = 0; i < values.Count; i++)
                    {
                        if (i < existingAttributeValues.Count)
                        {
                            existingAttributeValues[i].Value.Text = values[i];
                        }
                        else
                        {
                            // Handle the case where the number of new values is greater than existing ones
                            // You might want to add new values or handle it based on your specific requirements
                            break;
                        }
                    }

                    _context.UpdateRange(existingAttributeValues);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttributeExists(attribute.Id))
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
            return View(attribute);
        }


        
        // GET: Attributes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Attributes == null)
            {
                return NotFound();
            }

            var attribute = await _context.Attributes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (attribute == null)
            {
                return NotFound();
            }

            return View(attribute);
        }

        // POST: Attributes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Attributes == null)
            {
                return Problem("Entity set 'AppDbContext.Attributes'  is null.");
            }
            var attribute = await _context.Attributes.FindAsync(id);
            if (attribute != null)
            {
                _context.AttributeValues.RemoveRange(_context.AttributeValues.Where(av => av.AttributeId == attribute.Id));
                _context.Values.RemoveRange(_context.Values.Where(v => v.AttributeValues.Any(av => av.AttributeId == attribute.Id)));
                _context.Attributes.Remove(attribute);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AttributeExists(int id)
        {
          return (_context.Attributes?.Any(e => e.Id == id)).GetValueOrDefault();
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
