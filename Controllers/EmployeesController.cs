using DataTables.AspNetCore.Mvc.Binder;
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
            ViewBag.Projects = await _context.Projects.ToListAsync();
            ViewBag.Functions = await _context.Functions.ToListAsync();
            return View();
        }
        
        // POST: Employees/AddToEmployeeStock
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToEmployeeStock([Bind("Id,Date,StockIn,StockOut,Status,Remarks,VariantValueId,EmployeeId,ProjectId,FunctionId")] EmployeeStock employeeStock)
        {

            employeeStock.Status = StockEmployeeStatus.Current;
            //return Json(employeeStock);
            if (ModelState.IsValid)
            {
                _context.Add(employeeStock);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new {id = employeeStock.EmployeeId});
            }

            /*ViewBag.Category = new SelectList(_context.Categories, "Id", "Title", 
                employeeStock.VariantValue.Variant.CategoryId);*/

            Console.WriteLine(ViewBag.Category);
            
            /*ViewData["VariantValueId"] = new SelectList(_context.VariantValues, "Id", "Id", employeeStock.VariantValueId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FirstName", employeeStock.EmployeeId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Prefix", employeeStock.ProjectId);
            ViewData["FunctionId"] = new SelectList(_context.Functions, "Id", "Title", employeeStock.FunctionId);

            var errorList = (from item in ModelState.Values
                from error in item.Errors
                select error.ErrorMessage).ToList();*/
          //  return Json(errorList);
            
            return View();
        }

        // GET: Employees/
        
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
                .Include(e => e.StockEmployees)!
                .ThenInclude(e => e.Article)
                .ThenInclude(e => e.Ppe)
                .Include(e => e.StockEmployees)
                .ThenInclude(e => e.Article)
                .ThenInclude(e => e.AttributeValueAttributeCategory)
                .ThenInclude(e => e.AttributeValue)
                .ThenInclude(e => e.Value)
                .Include(e => e.Dotations)
                .ThenInclude(e => e.DotationDetails)
                .ThenInclude(e => e.Article)
                .ThenInclude(e => e.Ppe)
                .Include(e => e.Dotations)
                .ThenInclude(e => e.DotationDetails)
                .ThenInclude(e => e.Article)
                .ThenInclude(e => e.AttributeValueAttributeCategory)
                .ThenInclude(e => e.AttributeValue)
                .ThenInclude(e => e.Value)
                .Include(e => e.PayableStocks)
                .ThenInclude(e => e.Article)
                .ThenInclude(e => e.Ppe)
                .Include(e => e.PayableStocks)
                .ThenInclude(e => e.Article)
                .ThenInclude(e => e.AttributeValueAttributeCategory)
                .ThenInclude(e => e.AttributeValue)
                .ThenInclude(e => e.Value)
                .Include(e => e.Returns)
                .ThenInclude(e => e.Article)
                .ThenInclude(e => e.AttributeValueAttributeCategory)
                .ThenInclude(e => e.AttributeValue)
                .ThenInclude(e => e.Value)
                .FirstOrDefaultAsync(m => m.Id == id);

            var employeeStocks = await _context.EmployeeStocks
                .Include(e => e.PpeAttributeCategoryAttributeValue)
                .ThenInclude(e => e.Ppe)
                .Include(e => e.PpeAttributeCategoryAttributeValue)
                .ThenInclude(e => e.AttributeValueAttributeCategory)
                .ThenInclude(e => e.AttributeCategory)
                .ThenInclude(e => e.Attribute)
                .Include(e => e.PpeAttributeCategoryAttributeValue)
                .ThenInclude(e => e.AttributeValueAttributeCategory)
                .ThenInclude(e => e.AttributeValue)
                .ThenInclude(e => e.Value)
                .Where(e => e.EmployeeId == id)
                .OrderByDescending(e => e.Date)
                .ToListAsync();

            ViewBag.CurrentStock = employeeStocks
                .OrderByDescending(e => e.Date)
                .Where(e => e is { Status: StockEmployeeStatus.Current, IsArchived: false })
                .Select(e => new
                {
                    _Date = e.Date,
                    Id = e.Id,
                    Ppe = e.PpeAttributeCategoryAttributeValue.Ppe.Title,
                    Designation = e.Designation,
                    StockIn = e.StockIn,
                    StockOut = e.StockOut,
                    CurrentStock = e.StockIn - e.StockOut,
                    AttributeValue = e.PpeAttributeCategoryAttributeValue.AttributeValueAttributeCategory.AttributeValue.Value.Text
                });
            
            ViewBag.StockReturned = employeeStocks
                .Where(e => e.Status == StockEmployeeStatus.Returned
                            || e.Status == StockEmployeeStatus.Damaged
                            || e.Status == StockEmployeeStatus.Lost
                            || e.Status == StockEmployeeStatus.Current)
                .Select(e => new
                {
                    _Date = e.Date,
                    Id = e.Id,
                    Ppe = e.PpeAttributeCategoryAttributeValue.Ppe.Title,
                    Designation = e.Designation,
                    StockIn = e.StockIn,
                    StockOut = e.StockOut,
                    PpeCondition = e.PpeCondition,
                    CurrentStock = e.StockIn - e.StockOut,
                    AttributeValue = e.PpeAttributeCategoryAttributeValue.AttributeValueAttributeCategory.AttributeValue.Value.Text,
                    Status = e.Status,
                    
                });
            
            var stockToPay = _context.StocksToBePaid
                .Include(e => e.EmployeeStock)
                .ThenInclude(e => e.PpeAttributeCategoryAttributeValue)
                .ThenInclude(e => e.Ppe)
                .Include(e => e.EmployeeStock)
                .ThenInclude(e => e.PpeAttributeCategoryAttributeValue)
                .ThenInclude(e => e.AttributeValueAttributeCategory)
                .ThenInclude(e => e.AttributeCategory)
                .ThenInclude(e => e.Attribute)
                .Include(e => e.EmployeeStock)
                .ThenInclude(e => e.PpeAttributeCategoryAttributeValue)
                .ThenInclude(e => e.AttributeValueAttributeCategory)
                .ThenInclude(e => e.AttributeValue)
                .ThenInclude(e => e.Value)
                .Where(e => e.EmployeeStock.EmployeeId == id && e.IsPaid == false)
                .OrderByDescending(e => e.EmployeeStock.Date)
                .Select(e => new
                {
                    _Date = e.EmployeeStock.Date,
                    Id = e.EmployeeStock.Id,
                    Ppe = e.EmployeeStock.PpeAttributeCategoryAttributeValue.Ppe.Title,
                    Designation = e.EmployeeStock.Designation,
                    StockIn = e.EmployeeStock.StockIn,
                    StockOut = e.EmployeeStock.StockOut,
                    CurrentStock = e.EmployeeStock.StockIn - e.EmployeeStock.StockOut,
                    AttributeValue = e.EmployeeStock.PpeAttributeCategoryAttributeValue.AttributeValueAttributeCategory.AttributeValue.Value.Text,
                    Status = e.EmployeeStock.Status,
                    
                });
            
            ViewBag.StockToPay = stockToPay;
            ViewBag.StockToPayCount = stockToPay.Count();
            
            ViewBag.StockLostCount = employeeStocks
                .Count(e => e.Status == StockEmployeeStatus.Lost);
            
            ViewBag.StockLost = employeeStocks
                .Where(e => e.Status == StockEmployeeStatus.Lost)
                .OrderByDescending(e => e.Date)
                .Select(e => new
                {
                    _Date = e.Date,
                    Id = e.Id,
                    Ppe = e.PpeAttributeCategoryAttributeValue.Ppe.Title,
                    StockIn = e.StockIn,
                    StockOut = e.StockOut,
                    Designation = e.Designation,
                    CurrentStock = e.StockIn - e.StockOut,
                    AttributeValue = e.PpeAttributeCategoryAttributeValue.AttributeValueAttributeCategory.AttributeValue.Value.Text,
                    Status = e.Status
                });
            if (employee == null)
            {
                return NotFound();
            }
            
            ViewBag.Magaziniers = await _context.Managers
                .Where(u => u.Fonction == Fonction.Magasinier)
                .Where(u => u.ProjectId == employee.ProjectId)
                .ToListAsync();
            
            ViewBag.Responsables = await _context.Managers
                .Where(u => u.Fonction == Fonction.Responsable)
                .ToListAsync();
            
            ViewBag.Transporters = await _context.Managers
                .Where(u => u.Fonction == Fonction.Transporteur)
                .Where(u => u.ProjectId == employee.ProjectId)
                .ToListAsync();
            
            ViewBag.Hses = await _context.Managers
                .Where(u => u.Fonction == Fonction.HSE)
                .Where(u => u.ProjectId == employee.ProjectId)
                .ToListAsync();
            
            ViewBag.Coordinators = await _context.Managers
                .Where(u => u.Fonction == Fonction.Coordinateur)
                .Where(u => u.ProjectId == employee.ProjectId)
                .ToListAsync();
            
            ViewBag.StockStatus = new SelectList(Enum.GetValues(typeof(StockType)));

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
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Matricule,NNI,Phone,Tel,Gender,Size,ShoeSize,ProjectId,FunctionId")] Employee employee)
        {
            //return Json(employee);
            if (ModelState.IsValid)
            {
                //return Json(employee);
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            /*return Json(new
            {
                errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
            });
            */
            
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,NNI,Matricule,Phone,Tel,Gender,Size,ShoeSize,ProjectId,FunctionId")] Employee employee)
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

        // POST: Employees/ReturnPpe/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ReturnPpe(int id, int status, DateTime date)
        {

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var employeeStock = _context.EmployeeStocks.Find(id);
                    if (employeeStock == null)
                    {
                        return NotFound();
                    }
                    employeeStock.IsArchived = true;
                    _context.Update(employeeStock);
                    _context.SaveChanges();
                    
                    if (status == (int)PpeStatus.Lost)
                    {
                        var newEmployeeStock = new EmployeeStock
                        {
                            Date = date,
                            StockIn = employeeStock.StockIn,
                            StockOut = employeeStock.StockOut,
                            Status = StockEmployeeStatus.Lost,
                            Remarks = employeeStock.Remarks,
                            Designation = Designation.Lost,
                            PpeCondition = PpeCondition.Lost,
                            PpeAttributeCategoryAttributeValueId = employeeStock.PpeAttributeCategoryAttributeValueId,
                            EmployeeId = employeeStock.EmployeeId,
                            ProjectId = employeeStock.ProjectId,
                            FunctionId = employeeStock.FunctionId,
                            StockType = StockType.Lost,
                        };
                
                        _context.Add(newEmployeeStock);
                        _context.SaveChanges();
                    }
                    else if (status == (int)PpeStatus.Damaged)
                    {
                        var newStockEmployee = new EmployeeStock
                        {
                            Date = date,
                            StockIn = employeeStock.StockIn,
                            StockOut = employeeStock.StockOut,
                            Status = StockEmployeeStatus.Damaged,
                            Remarks = employeeStock.Remarks,
                            Designation = Designation.Return,
                            PpeCondition = PpeCondition.Damaged,
                            PpeAttributeCategoryAttributeValueId = employeeStock.PpeAttributeCategoryAttributeValueId,
                            EmployeeId = employeeStock.EmployeeId,
                            ProjectId = employeeStock.ProjectId,
                            FunctionId = employeeStock.FunctionId,
                            StockType = StockType.Return,
                        };
                        _context.Add(newStockEmployee);
                        _context.SaveChanges();
                    }
                    else
                    {
                        var newStockEmployee = new EmployeeStock
                        {
                            Date = date,
                            StockIn = employeeStock.StockIn,
                            StockOut = employeeStock.StockOut,
                            Status = StockEmployeeStatus.Returned,
                            Remarks = employeeStock.Remarks,
                            Designation = Designation.Return,
                            PpeCondition = PpeCondition.Good,
                            PpeAttributeCategoryAttributeValueId = employeeStock.PpeAttributeCategoryAttributeValueId,
                            EmployeeId = employeeStock.EmployeeId,
                            ProjectId = employeeStock.ProjectId,
                            FunctionId = employeeStock.FunctionId,
                            StockType = StockType.Return,
                        };
                        _context.Add(newStockEmployee);
                        _context.SaveChanges();
                    }
                    transaction.Commit();
                    return RedirectToAction("Details", new {id = employeeStock.EmployeeId});
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    transaction.Rollback();
                    throw;
                }
            }
        }
        
         // GET: Ppes/api
        [HttpGet()]
        [Route("api/employees")]
        public IActionResult Get([DataTablesRequest] DataTablesRequest dataRequest)
        {
            //IEnumerable<Ppe> ppe = _context.Ppes.Include(p => p.Category);
            IQueryable<Employee> employees = _context.Employees
                .Include(p => p.Project)
                .Include(p => p.Function);
            
            int recordsTotal = employees.Count();
            int recordsFilterd = recordsTotal;

            var filters = new Dictionary<string, string>();
            foreach(var key in Request.Query.Keys)
            {
                if (key.StartsWith("search"))
                {
                    filters[key.Substring("search".Length)] = Request.Query[key];
                    if (key == "search[value]" && !string.IsNullOrEmpty(Request.Query[key]))
                    {
                        employees = employees.Where(e => e.FirstName.Contains(Request.Query[key].ToString()) 
                                                         || e.LastName.Contains(Request.Query[key].ToString()) 
                                                         || e.NNI.Contains(Request.Query[key].ToString()) 
                                                         || e.Phone.Contains(Request.Query[key].ToString()) 
                                                         || e.Matricule.Contains(Request.Query[key].ToString())
                        );
                        recordsFilterd = employees.Count();
                    }
                }
                
                if (key.StartsWith("filters"))
                {
                    filters[key.Substring("filters".Length)] = Request.Query[key];
                    
                    if (key == "filters[project]" && !string.IsNullOrEmpty(Request.Query[key]))
                    {
                        employees = employees.Where(e => e.ProjectId == int.Parse(Request.Query[key]));
                        recordsFilterd = employees.Count();
                    }
                    if (key == "filters[function]" && !string.IsNullOrEmpty(Request.Query[key]))
                    {
                        employees = employees.Where(e => e.FunctionId == int.Parse(Request.Query[key]));
                        recordsFilterd = employees.Count();
                    }

                    if (key == "filters[name]" && !string.IsNullOrEmpty(Request.Query[key]))
                    {
                        employees = employees.Where(e => e.FirstName.Contains(Request.Query[key].ToString()) 
                                                         || e.LastName.Contains(Request.Query[key].ToString()) 
                                                         || e.NNI.Contains(Request.Query[key].ToString()) 
                                                         || e.Phone.Contains(Request.Query[key].ToString()) 
                        );
                        recordsFilterd = employees.Count();
                    }
                }
            }
            
            employees = employees.Skip(dataRequest.Start).Take(dataRequest.Length);
            var deleteUrl = "Employees/DeleteEmployee";
            
            return Json(employees
                .Select(e => new
                {
                    Id = e.Id,
                    Matricule = e.Matricule,
                    FullName = e.FullName,
                    NNI = e.NNI,
                    Phone = e.Phone,
                    Project = e.Project.Title,
                    Function = e.Function.Title,
                    Actions = $"<div class='btn-group'>" +
                              $"<a class='btn btn-primary btn-sm' href='/Employees/Edit/{e.Id}'>" +
                              $"<i class='la la-edit'></i>"+
                              $"</a>"+
                              $"<a class='btn btn-dark btn-sm' href='/Employees/Details/{e.Id}'>" +
                              $"<i class='la la-eye'></i>"+
                              $"</a>"+
                              $"<a class='btn btn-danger btn-sm' onclick='confirmDelete(\"{deleteUrl}\", {e.Id})'>" +
                              $"<i class='la la-trash text-white'></i>"+
                              $"</a>"+
                              $"</div>"
                })
                .ToDataTablesResponse(dataRequest, recordsTotal, recordsFilterd));
        }

        public IActionResult EditReturnPpe(int id, int status, DateTime date)
        {
            
            var employeeStock = _context.EmployeeStocks.Find(id);
            if (employeeStock == null)
            {
                return NotFound();
            }

            employeeStock.IsArchived = true;
            _context.Update(employeeStock);
            _context.SaveChanges();

            if (status == (int)PpeStatus.Lost)
            {
                employeeStock.Date = date;
                employeeStock.StockIn = employeeStock.StockIn;
                employeeStock.StockOut = employeeStock.StockOut;
                employeeStock.Status = StockEmployeeStatus.Lost;
                employeeStock.Remarks = employeeStock.Remarks;
                employeeStock.Designation = Designation.Lost;
                employeeStock.PpeCondition = PpeCondition.Lost;
                employeeStock.PpeAttributeCategoryAttributeValueId = employeeStock.PpeAttributeCategoryAttributeValueId;
                employeeStock.EmployeeId = employeeStock.EmployeeId;
                employeeStock.ProjectId = employeeStock.ProjectId;
                employeeStock.FunctionId = employeeStock.FunctionId;
                employeeStock.StockType = StockType.Lost;
                employeeStock.PpeCondition = PpeCondition.Lost;

                _context.Update(employeeStock);
                _context.SaveChanges();
            }
            else if (status == (int)PpeStatus.Damaged)
            {
                employeeStock.Date = date;
                employeeStock.StockIn = employeeStock.StockIn;
                employeeStock.StockOut = employeeStock.StockOut;
                employeeStock.Status = StockEmployeeStatus.Damaged;
                employeeStock.Remarks = employeeStock.Remarks;
                employeeStock.Designation = Designation.Return;
                employeeStock.PpeCondition = PpeCondition.Damaged;
                employeeStock.PpeAttributeCategoryAttributeValueId = employeeStock.PpeAttributeCategoryAttributeValueId;
                employeeStock.EmployeeId = employeeStock.EmployeeId;
                employeeStock.ProjectId = employeeStock.ProjectId;
                employeeStock.FunctionId = employeeStock.FunctionId;
                employeeStock.StockType = StockType.Return;
                employeeStock.PpeCondition = PpeCondition.Damaged;
                _context.Update(employeeStock);
                _context.SaveChanges();
            }
            else
            {
                employeeStock.Date = date;
                employeeStock.StockIn = employeeStock.StockIn;
                employeeStock.StockOut = employeeStock.StockOut;
                employeeStock.Status = StockEmployeeStatus.Returned;
                employeeStock.Remarks = employeeStock.Remarks;
                employeeStock.Designation = Designation.Return;
                employeeStock.PpeCondition = PpeCondition.Good;
                employeeStock.PpeAttributeCategoryAttributeValueId = employeeStock.PpeAttributeCategoryAttributeValueId;
                employeeStock.EmployeeId = employeeStock.EmployeeId;
                employeeStock.ProjectId = employeeStock.ProjectId;
                employeeStock.FunctionId = employeeStock.FunctionId;
                employeeStock.StockType = StockType.Return;
                employeeStock.PpeCondition = PpeCondition.Good;
                
                _context.Update(employeeStock);
                _context.SaveChanges();
                //return Json(new {id, status, date, employeeStock});
                // returned to the stock of this employee project
                
                var ppeId = _context.PpeAttributeCategoryAttributeValues
                    .Find(employeeStock.PpeAttributeCategoryAttributeValueId).PpeId;
                
                var newStockReturned = new Stock
                {
                    ProjectId = employeeStock.ProjectId,
                    Date = employeeStock.Date,
                    StockIn = employeeStock.StockIn,
                    StockType = StockType.Normal,
                    PpeId = ppeId,
                    StockNature = StockNature.Project,
                };
                _context.Add(newStockReturned);
                _context.SaveChanges();
                
                //return Json(newStockReturned);

                var newStockDetailedReturned = new StockDetail
                {
                    StockId = newStockReturned.Id,
                    PpeAttributeCategoryAttributeValueId = (int)employeeStock.PpeAttributeCategoryAttributeValueId,
                    StockIn = employeeStock.StockIn,
                };

                _context.Add(newStockDetailedReturned);
                _context.SaveChanges();

            }

            return RedirectToAction("Details", new { id = employeeStock.EmployeeId });
        }
        
        // GET: api/Employees/5
        [HttpGet()]
        [Route("api/employees/{id}")]
        public async Task<ActionResult<Employee>> Get(int id)
        {
            if (_context.Employees == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Employees'  is null.");
            }
            var employee = await _context.Employees
                .Include(e => e.Function)
                .Include(e => e.Project)
                .FirstOrDefaultAsync(e => e.Id == id);
            
            if (employee == null)
            {
                return NotFound();
            }
            
            return employee;
        }
        
        // GET: Employees/DeleteEmployee/5
        [HttpGet()]
        [Route("Employees/DeleteEmployee/{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
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
            return Json(new {success = true, message = "Employee deleted successfully"});
        }
    }
}
