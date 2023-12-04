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
}