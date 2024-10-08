using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserAuth.Data;
using UserAuth.ViewModels;

namespace UserAuth.Controllers
{
    public class ReportController : Controller
    {
        private readonly AppDbContext _context;

        public ReportController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Report/InventoryReport
        public async Task<IActionResult> InventoryReport()
        {
            var products = await _context.Products.ToListAsync();

            var report = new InventoryReportViewModel
            {
                InventoryName = "Main Inventory", // Example inventory name
                ReportGeneratedTime = DateTime.Now,
                Products = products.Select(p => new ProductInfo
                {
                    ProductName = p.ProductName,
                    Quantity = p.Quantity,
                    Price = p.Price
                }).ToList(),
                TotalInventoryValue = products.Sum(p => p.Quantity * p.Price)
            };

            return View(report); // Return report data to view
        }

    }
}