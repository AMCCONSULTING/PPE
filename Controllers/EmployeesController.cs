using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PPE.Data;
using PPE.Data.Enums;
using PPE.Models;

namespace PPE.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly AppDbContext _context;

        public EmployeesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Employees.Include(e => e.Function).Include(e => e.Project);
            ViewData["FunctionId"] = new SelectList(_context.Functions, "Id", "Title");
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Prefix");
            return View(await applicationDbContext.ToListAsync());
        }
        
        // GET: Employees/AddToEmployeeStock/5
        public IActionResult AddToEmployeeStock(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = _context.Employees
                .Include(e => e.Function)
                .Include(e => e.Project)
                .FirstOrDefault(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewBag.VariantValues = _context.VariantValues
                .Include(v => v.Variant)
                .ThenInclude(v => v.Ppe)
                .Include(v => v.Value)
                .Select(v => new SelectListItem
                {
                    Text = $"{v.Variant.Ppe.Title} - {v.Value.Text}",
                    Value = v.Id.ToString(),
                });
            ViewBag.EmployeeId = employee.Id;
            ViewBag.ProjectId = employee.ProjectId;
            ViewBag.FunctionId = employee.FunctionId;
            return View(employee);
        }
        
        // POST: Employees/AddToEmployeeStock
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToEmployeeStock([Bind("Id,Date,StockIn,StockOut,Status,Remarks,VariantValueId,EmployeeId,ProjectId,FunctionId")] EmployeeStock employeeStock)
        {
            employeeStock.Status = StockEmployeeStatus.Current;
            employeeStock.Id = 5;
            //return Json(employeeStock);
            if (ModelState.IsValid)
            {
                _context.Add(employeeStock);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new {id = employeeStock.EmployeeId});
            }
            ViewData["VariantValueId"] = new SelectList(_context.VariantValues, "Id", "Id", employeeStock.VariantValueId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FirstName", employeeStock.EmployeeId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Prefix", employeeStock.ProjectId);
            ViewData["FunctionId"] = new SelectList(_context.Functions, "Id", "Title", employeeStock.FunctionId);

            var errorList = (from item in ModelState.Values
                from error in item.Errors
                select error.ErrorMessage).ToList();
            return Json(errorList);
            
            return View();
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Function)
                .Include(e => e.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            var employeeStocks = await _context.EmployeeStocks
                .Include(e => e.Employee)
                .Include(e => e.Function)
                .Include(e => e.Project)
                .Include(e => e.VariantValue)
                .ThenInclude(e => e.Variant)
                .ThenInclude(e => e.Ppe)
                .Include(e => e.VariantValue)
                .ThenInclude(e => e.Value)
                .Where(e => e.EmployeeId == id && e.Status == StockEmployeeStatus.Current)
                .ToListAsync();
            
            var employeeStocksHistory = await _context.EmployeeStocks
                .Include(e => e.Employee)
                .Include(e => e.Function)
                .Include(e => e.Project)
                .Include(e => e.VariantValue)
                .ThenInclude(e => e.Variant)
                .ThenInclude(e => e.Ppe)
                .Include(e => e.VariantValue)
                .ThenInclude(e => e.Value)
                .Where(e => e.EmployeeId == id && e.Status == StockEmployeeStatus.Returned
                            || e.EmployeeId == id && e.Status == StockEmployeeStatus.Lost 
                            || e.EmployeeId == id && e.Status == StockEmployeeStatus.Damaged)
                .ToListAsync();
            
            ViewData["EmployeeStocksHistory"] = employeeStocksHistory;
            
            ViewData["EmployeeStocks"] = employeeStocks;
            
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            ViewData["Size"] = new SelectList(Enum.GetValues(typeof(Size)));
            ViewData["ShoeSize"] = new SelectList(Enum.GetValues(typeof(ShoeSize)));
            ViewData["FunctionId"] = new SelectList(_context.Functions, "Id", "Title");
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Title");
            ViewData["Gender"] = new SelectList(Enum.GetValues(typeof(Gender)));

            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,NNI,Phone,Tel,Gender,Size,ShoeSize,ProjectId,FunctionId")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Size"] = new SelectList(Enum.GetValues(typeof(Size)));
            ViewData["ShoeSize"] = new SelectList(Enum.GetValues(typeof(ShoeSize)));
            ViewData["FunctionId"] = new SelectList(_context.Functions, "Id", "Title", employee.FunctionId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Prefix", employee.ProjectId);
            ViewData["Gender"] = new SelectList(Enum.GetValues(typeof(Gender)));
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["Size"] = new SelectList(Enum.GetValues(typeof(Size)));
            ViewData["ShoeSize"] = new SelectList(Enum.GetValues(typeof(ShoeSize)));
            ViewData["FunctionId"] = new SelectList(_context.Functions, "Id", "Title", employee.FunctionId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Prefix", employee.ProjectId);
            ViewData["Gender"] = new SelectList(Enum.GetValues(typeof(Gender)));
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,NNI,Phone,Tel,Gender,Size,ShoeSize,ProjectId,FunctionId")] Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
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
            ViewData["Size"] = new SelectList(Enum.GetValues(typeof(Size)));
            ViewData["ShoeSize"] = new SelectList(Enum.GetValues(typeof(ShoeSize)));
            ViewData["FunctionId"] = new SelectList(_context.Functions, "Id", "Title", employee.FunctionId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Prefix", employee.ProjectId);
            ViewData["Gender"] = new SelectList(Enum.GetValues(typeof(Gender)));
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Function)
                .Include(e => e.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Employees == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Employees'  is null.");
            }
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
          return (_context.Employees?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
