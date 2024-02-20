using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using OfficeOpenXml;
using PPE.Data;
using PPE.Data.Services;
using PPE.Models;

namespace PPE.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    //private readonly SharePointService _sharePointService;

    public HomeController(ILogger<HomeController> logger, AppDbContext context, IConfiguration configuration)
    {
        _logger = logger;
        _context = context;
        _configuration = configuration;
        //_sharePointService = sharePointService;
    }

    public IActionResult Index()
    {
        /*var stocks = _context.Stocks
            .Include(s => s.Ppe)
            .Where(s => s.ProjectId == null && s.StockType == StockType.Normal)
            .GroupBy(s => s.Ppe);
        
        // get ppes that are below threshold
        var ppeBelowThreshold = _context.Stocks
            .Include(s => s.Project)
            .Include(v => v!.Ppe)
            .Where(s => s.StockIn - s.StockOut <= s!.Ppe!.Threshold)
            .Where(s => s.ProjectId == null && s.StockType == StockType.Normal)
            .GroupBy(s => s.Ppe.Title)
            .Select(g => new
            {
                Ppe = g.Key,
                StockIn = g.Sum(s => s.StockIn),
                StockOut = g.Sum(s => s.StockOut),
                CurrentStock = g.Sum(s => s.StockIn) - g.Sum(s => s.StockOut),
            });*/
        
        var mainStocks = _context.MainStocks
            .Include(s => s.PpeAttributeCategoryAttributeValue)
            .ThenInclude(p => p.Ppe)
            .GroupBy(s => s.PpeAttributeCategoryAttributeValue.Ppe.Category);
        
        var stockOuts = mainStocks.Select(s => s.Sum(sg => sg.QuantityOut)).ToList();
        var stockIns = mainStocks.Select(s => s.Sum(sg => sg.QuantityIn)).ToList();
        var currentStocks = mainStocks.Select(s => s.Sum(sg => sg.QuantityIn - sg.QuantityOut)).ToList();
        /*var labels = stocks.Select(s => $"{s.Key.Title}").ToList();
        var data = stocks.Select(s => s.Sum(s => s.StockIn - s.StockOut)).ToList();*/
        ViewBag.Labels = mainStocks.Select(s => $"{s.Key!.Title}").ToList();
        ViewBag.Data = currentStocks;
        ViewBag.StockIns = stockIns;
        ViewBag.StockOuts = stockOuts;
        
       // group in save array of objects
       ViewBag.Stock = mainStocks.Select(s => new
       {
              Ppe = s.Key!.Title,
              StockIn = s.Sum(sg => sg.QuantityIn),
              StockOut = s.Sum(sg => sg.QuantityOut),
              CurrentStock = s.Sum(s => s.QuantityIn - s.QuantityOut),
       });
        
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public FileResult Export()
    {
        var stockDetails = _context.MainStocks
            .Include(s => s.PpeAttributeCategoryAttributeValue)
            .ThenInclude(p => p!.Ppe)
            .GroupBy(s => s.PpeAttributeCategoryAttributeValue.Ppe)
            .Select(s => new
            {
                Ppe = s.Key!.Title,
                StockIn = s.Sum(sg => sg.QuantityIn),
                StockOut = s.Sum(sg => sg.QuantityOut),
                CurrentStock = s.Sum(s => s.QuantityIn - s.QuantityOut),
            }).ToList();
        
        var sb = new StringBuilder();
        sb.AppendLine("PPE,Stock In,Stock Out,Current Stock");
        foreach (var stockDetail in stockDetails)
        {
            sb.AppendLine($"{stockDetail.Ppe},{stockDetail.StockIn},{stockDetail.StockOut},{stockDetail.CurrentStock}");
        }
        
        return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "stock.csv");
        
    }

    
    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        // Obtain access token using MSAL
        var accessToken = await GetAccessToken();

        /*return Json(new
        {
            accessToken = accessToken,
            fileName = file.FileName,
        });*/

        // Specify SharePoint site URL and folder path
        // https://amcct.sharepoint.com/sites/ppe.documents/SitePages/CollabHome.aspx
        var siteUrl = "https://amcct.sharepoint.com/sites/ppe.documents";
        var folderPath = "/Shared Documents/Forms/AllItems.aspx?id=%2Fsites%2Fppe%2Edocuments%2FShared%20Documents%2FDotations&viewid=d4ea92da%2Dda2a%2D433e%2D8fd8%2D16e87ab1c4cd";

        // Upload file to SharePoint
        var sharePointService = new SharePointService(accessToken, siteUrl, folderPath);
        await sharePointService.UploadFileAsync(file.OpenReadStream(), file.FileName);

        return RedirectToAction("Index");
    }

    private async Task<string> GetAccessToken()
    {
        // Obtain access token using MSAL
        var scopes = new[] { "https://amcct.sharepoint.com/.default" };
        var tenantId = "2bb37157-5ae3-40a9-b823-fb07fcaeaffa";
        var clientId = "d6d87276-54a4-49fb-8cbd-cd530655ae12";
        var clientSecret = "a5P8Q~CZVEQQIPt_gAMzJclVEUkpQ.4tL783Qdrx";
        var confidentialClient = ConfidentialClientApplicationBuilder
            .Create(clientId)
            .WithClientSecret(clientSecret)
            .WithAuthority(new Uri($"https://login.microsoftonline.com/{tenantId}"))
            .Build();        
        return (await confidentialClient.AcquireTokenForClient(scopes).ExecuteAsync()).AccessToken;
    }
    
    [HttpPost]
    public IActionResult UploadFile()
    {
        var file = HttpContext.Request.Form.Files["file"];
        
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
                    var project = _context.Projects.FirstOrDefault(p => p.Title == worksheet.Cells[row, 1].Value.ToString());
                    if (project != null)
                    {
                        continue;
                    }
                    var data = new Project
                    {
                        Prefix = worksheet.Cells[row, 3].Value?.ToString(),
                        Title = worksheet.Cells[row, 1].Value?.ToString(),
                        Description = worksheet.Cells[row, 2].Value?.ToString(),
                    };
                    _context.Projects.Add(data);
                }
                _context.SaveChanges();
            }
            transaction.Commit();
            return RedirectToAction("Index");
        }
        catch (DbUpdateException ex)
        {
            transaction.Rollback();
            var errorHandlingService = new ErrorHandlingService();
            ModelState.AddModelError("", errorHandlingService.GetFullErrorMessage(ex));
            //ModelState.AddModelError("", GetFullErrorMessage(ex));
            return View("Index");
        }

        /*if (file is { Length: > 0 })
        {
            using var stream = new MemoryStream();
            file.CopyTo(stream);
            using var package = new ExcelPackage(stream);
            var worksheet = package.Workbook.Worksheets[0];
            
            for (int row = 2; row <= worksheet.Dimension.Rows; row++)
            {
                var data = new Project
                {
                    Prefix = worksheet.Cells[row, 3].Value.ToString(),
                    Title = worksheet.Cells[row, 1].Value.ToString(),
                    Description = worksheet.Cells[row, 2].Value.ToString(),
                    // Map other columns accordingly
                };
                _context.Projects.Add(data);

               //return Json(data);
            }
            _context.SaveChanges();
        }*/
    }
    
    /*private string GetFullErrorMessage(DbUpdateException ex)
    {
        var messages = new List<string>();
        Exception currentException = ex;

        while (currentException != null)
        {
            messages.Add(currentException.Message);
            currentException = currentException.InnerException;
        }

        return string.Join(" ", messages);
    }*/
    
}

