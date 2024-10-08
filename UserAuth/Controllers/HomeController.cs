using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Diagnostics;
using UserAuth.Data;
using UserAuth.Models;
using UserAuth.ViewModels;

namespace UserAuth.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        // Single constructor that accepts both ILogger and AppDbContext
        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Fetch Total Inventory Value
            var totalInventoryValue = await _context.Products
                .SumAsync(p => p.Price * p.Quantity);

            // Count total number of products
            var totalProducts = await _context.Products.CountAsync();

            // Count total number of categories
            var totalCategories = await _context.Categories.CountAsync();

            // Fetch number of products per category
            var categoryProductCounts = await _context.Categories
                .Select(c => new CategoryProductCount
                {
                    CategoryName = c.CategoryName,
                    ProductCount = c.Products.Count()
                })
                .ToListAsync();

            // Create the view model
            var model = new DashboardViewModel
            {
                TotalInventoryValue = totalInventoryValue,
                TotalProducts = totalProducts,
                TotalCategories = totalCategories,
                CategoryProductCounts = categoryProductCounts
            };

            // Log the index page access
            _logger.LogInformation("Index page accessed at {Time}", DateTime.Now);

            return View(model);
        }

        public IActionResult Privacy()
        {
            // Log the privacy page access using Serilog
            Log.Information("Privacy page accessed at {Time}", DateTime.Now);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // Log error and display error page
            _logger.LogError("An error occurred at {Time}", DateTime.Now);
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
