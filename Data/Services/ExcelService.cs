using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using PPE.Models;

namespace PPE.Data.Services;

public class ExcelService : IExcelService
{
    private readonly AppDbContext _context;

    public ExcelService(AppDbContext context)
    {
        _context = context;
    }

    public void InsertFunctionsFromExcel(string filePath)
    {
        FileInfo fileInfo = new FileInfo(filePath);

        using (ExcelPackage package = new ExcelPackage(fileInfo))
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

            for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
            {
                string title = worksheet.Cells[row, 1].Value?.ToString(); // Assuming title is in the first column

                if (!string.IsNullOrEmpty(title))
                {
                    var function = new Function
                    {
                        Title = title,
                        // Set other properties as needed
                    };

                    _context.Functions.Add(function);
                }
            }

            _context.SaveChanges();
        }
    }
    
    /*public IActionResult UploadFile<TEntity>(Func<ExcelWorksheet, TEntity> mapRowFunc, IFormFile file) where TEntity : class
    {
        //var file = HttpContext.Request.Form.Files["file"];

        using var transaction = _context.Database.BeginTransaction();
        try
        {
            if (file is { Length: > 0 })
            {
                using var stream = new MemoryStream();
                file.CopyTo(stream);
                using var package = new ExcelPackage(stream);
                var worksheet = package.Workbook.Worksheets[0];

                for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                {
                    // Create an instance of TEntity using the provided mapping function
                    var entity = mapRowFunc(worksheet);

                    // Check if the entity already exists in the database
                    var existingEntity = _context.Set<TEntity>().Find(entity);

                    if (existingEntity != null)
                    {
                        continue;
                    }

                    // Add the entity to the context
                    _context.Set<TEntity>().Add(entity);
                }

                // Save changes to the database
                _context.SaveChanges();
            }

            // Commit the transaction if everything is successful
            transaction.Commit();
            return RedirectToAction("Index");
        }
        catch (DbUpdateException ex)
        {
            // Rollback the transaction in case of an exception
            transaction.Rollback();
            var errorHandlingService = new ErrorHandlingService();
            ModelState.AddModelError("", errorHandlingService.GetFullErrorMessage(ex));
            return View("Index");
        }
    }
    */

}