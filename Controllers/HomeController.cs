using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PPE.Data;
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
        // get ppe that are threshold or below threshold and send them to the view
        var ppe = _context.Stocks
            .Include(s => s.Project)
            .Include(s => s.VariantValue)
            .ThenInclude(v => v!.Variant)
            .ThenInclude(v => v.Ppe)
            //.Where(s => s.StockIn - s.StockOut <= s.VariantValue!.Variant.Ppe.Threshold)
            .ToList();
        
        // add the ppe that are not in the stock table
        var ppeNotInStock = _context.VariantValues
            .Include(v => v.Variant)
            .ThenInclude(v => v.Ppe)
            .Where(v => !ppe.Select(s => s.VariantValueId).Contains(v.Id))
            .Select(v => new Stock
            {
                StockIn = 0,
                StockOut = 0,
                ProjectId = 0,
                VariantValueId = v.Id,
                VariantValue = v
            })
            .ToList();
        
        ppe.AddRange(ppeNotInStock);
        
        ViewBag.Ppe = ppe;
        var labels = ppe.Select(s => $"{s.VariantValue.Variant.Ppe.Title} - {s.VariantValue.Value}").ToList();
        var data = ppe.Select(s => s.StockIn - s.StockOut).ToList();

        ViewBag.Labels = labels;
        ViewBag.Data = data;
        
        Console.WriteLine($"Labels: {string.Join(", ", labels)}");
        Console.WriteLine($"Data: {string.Join(", ", data)}");
        
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

