using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Mvc.Models;
using Packt.Shared;
using System.Diagnostics;

namespace Northwind.Mvc.Controllers;
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly NorthwindContext _db;

    public HomeController(ILogger<HomeController> logger, NorthwindContext db)
    {
        _logger = logger;
        _db = db;
    }

    public IActionResult Index(string? id = null, string? country = null)
    {
        IEnumerable<Order> model = _db.Orders
            .Include(order => order.Customer)
            .Include(order => order.OrderDetails);

        if (id != null)
        {
            model = model.Where(order => order.Customer?.CustomerId == id);
        }

        if (id != null)
        {
            model = model.Where(order => order.Customer?.CustomerId == country);
        }

        model = model
            .OrderByDescending(order => order.OrderDetails
                .Sum(detail => detail.Quantity * detail.UnitPrice))
            .AsEnumerable();

        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }
    
    public IActionResult Shipper(Shipper shipper)
    {
        return View(shipper);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ProcessShipper(Shipper shipper)
    {
        return Json(shipper);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
