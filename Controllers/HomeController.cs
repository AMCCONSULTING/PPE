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
        var stocks = _context.Stocks
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
            });
        
        
        
        var labels = stocks.Select(s => $"{s.Key.Title}").ToList();
        var data = stocks.Select(s => s.Sum(s => s.StockIn - s.StockOut)).ToList();
        ViewBag.Labels = labels;
        ViewBag.Data = data;
        
        // get ppe that are threshold or below threshold
        ViewBag.Ppe = ppeBelowThreshold;
        
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

