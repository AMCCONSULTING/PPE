using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PPE.Data;
using PPE.Data.Enums;
using PPE.Models;

namespace PPE.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _context;

    public HomeController(ILogger<HomeController> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
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
            .ThenInclude(p => p!.Ppe)
            .GroupBy(s => s.PpeAttributeCategoryAttributeValue!.Ppe.Category);
        
        var stockOuts = mainStocks.Select(s => s.Sum(s => s.QuantityOut)).ToList();
        var stockIns = mainStocks.Select(s => s.Sum(s => s.QuantityIn)).ToList();
        var currentStocks = mainStocks.Select(s => s.Sum(s => s.QuantityIn - s.QuantityOut)).ToList();
        /*var labels = stocks.Select(s => $"{s.Key.Title}").ToList();
        var data = stocks.Select(s => s.Sum(s => s.StockIn - s.StockOut)).ToList();*/
        ViewBag.Labels = mainStocks.Select(s => $"{s.Key.Title}").ToList();
        ViewBag.Data = currentStocks;
        ViewBag.StockIns = stockIns;
        ViewBag.StockOuts = stockOuts;
        
       // group in save array of objects
       ViewBag.Stock = mainStocks.Select(s => new
       {
              Ppe = s.Key.Title,
              StockIn = s.Sum(s => s.QuantityIn),
              StockOut = s.Sum(s => s.QuantityOut),
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
}

