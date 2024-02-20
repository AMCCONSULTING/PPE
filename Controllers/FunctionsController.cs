using DataTables.AspNetCore.Mvc.Binder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PPE.Data;
using PPE.Data.Services;
using PPE.Models;

namespace PPE.Controllers
{
    public class FunctionsController : Controller
    {
        private readonly AppDbContext _context;

        public FunctionsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Fucntions
        public async Task<IActionResult> Index()
        {
            var functions = _context.Functions
                .Include(e => e.Employees)
                .ToList();
            
            // get functions in list and separate by comma like this: "function1, function2, function3"
            /*ViewBag.Functions = functions.Select(f => f.Title).ToList();
            return Json(String.Join(", ", ViewBag.Functions));*/
            
              return View(await _context.Functions
                  .Include(e => e.Employees)
                  .ToListAsync());
        }
        
         [HttpGet()]
                [Route("api/functions")]
                public IActionResult Get([DataTablesRequest] DataTablesRequest dataRequest)
                {
                    //IEnumerable<Ppe> ppe = _context.Ppes.Include(p => p.Category);
                    IQueryable<Function> functions = _context.Functions
                        .OrderByDescending(e => e.Title);
                    int recordsTotal = functions.Count();
                    int recordsFilterd = recordsTotal;
                    
                    functions = functions.Skip(dataRequest.Start).Take(dataRequest.Length);
                    var deleteUrl = "Functions/DeleteFunction";
                    
                    return Json(functions
                        .Select(e => new
                        {
                            Id = e.Id,
                            Title = e.Title,
                            Description = e.Description,
                            EmployeeCount =  $"{e.Employees!.Count}  <a href='/Employees?filters[function]={e.Id}'> <i class='la la-eye'></i> </a>",
                            CreatedAt = e.CreatedAt,
                            CreatedBy = e.CreatedBy,
                            UpdatedAt = e.UpdatedAt,
                            UpdatedBy = e.UpdatedBy,
                            Actions = $"<div class='btn-group'>" +
                                      $"<a class='btn btn-primary btn-sm' href='/Functions/Edit/{e.Id}'>" +
                                      $"<i class='la la-edit'></i>"+
                                      $"</a>"+
                                      /*$"<a class='btn btn-dark btn-sm' href='/Employees/Details/{e.Id}'>" +
                                      $"<i class='la la-eye'></i>"+
                                      $"</a>"+
                                      $"<a class='btn btn-danger btn-sm' onclick='confirmDelete(\"{deleteUrl}\", {e.Id})'>" +
                                      $"<i class='la la-trash text-white'></i>"+
                                      $"</a>"+*/
                                      $"<a class='btn btn-danger btn-sm' onclick='confirmDelete(\"{deleteUrl}\", {e.Id})'>" +
                                      $"<i class='la la-trash text-white'></i>"+
                                      $"</a>"+
                                      $"</div>"
                        })
                        .ToDataTablesResponse(dataRequest, recordsTotal, recordsFilterd));
                }
                
                [HttpGet()]
                [Route("Functions/DeleteFunction/{id}")]
                public async Task<IActionResult> DeleteFunction(int id)
                {
                    if (_context.Functions == null)
                    {
                        return Problem("Entity set 'ApplicationDbContext.Function'  is null.");
                    }
                    var function = await _context.Functions.FindAsync(id);
                    if (function != null)
                    {
                        _context.Functions.Remove(function);
                    }
            
                    await _context.SaveChangesAsync();
                    return Json(new {success = true, message = "Function deleted successfully"});
                }

        // GET: Fucntions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Functions == null)
            {
                return NotFound();
            }

            var function = await _context.Functions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (function == null)
            {
                return NotFound();
            }

            return View(function);
        }

        // GET: Fucntions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Fucntions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,CreatedAt,UpdatedAt,CreatedBy")] Function function)
        {
            
            // if function title is already used then add error to model state and return to view
            
            if (_context.Functions != null && _context.Functions.Any(e => e.Title.ToUpper() == function.Title.ToUpper()))
            {
                ModelState.AddModelError("Title", "Function title already used.");
            }
            
            if (ModelState.IsValid)
            {
                /*function.CreatedAt = DateTime.Now;
                function.UpdatedAt = DateTime.Now;
                function.CreatedBy = User.Identity?.Name;
                function.UpdatedBy = User.Identity?.Name;*/
                _context.Add(function);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(function);
        }

        // GET: Fucntions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Functions == null)
            {
                return NotFound();
            }

            var function = await _context.Functions.FindAsync(id);
            if (function == null)
            {
                return NotFound();
            }
            return View(function);
        }

        // POST: Fucntions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,CreatedAt,UpdatedAt,CreatedBy,UpdatedBy")] Function function)
        {
            if (id != function.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    /*function.UpdatedAt = DateTime.Now;
                    function.UpdatedBy = User.Identity?.Name;*/
                    _context.Functions.Update(function);    
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FunctionExists(function.Id))
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
            return View(function);
        }

        // GET: Fucntions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Functions == null)
            {
                return NotFound();
            }

            var function = await _context.Functions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (function == null)
            {
                return NotFound();
            }

            return View(function);
        }

        // POST: Fucntions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Functions == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Functions'  is null.");
            }
            var function = await _context.Functions.FindAsync(id);
            if (function != null)
            {
                _context.Functions.Remove(function);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FunctionExists(int id)
        {
          return (_context.Functions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
