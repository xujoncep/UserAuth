using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserAuth.Data;
using UserAuth.ViewModels;

namespace UserAuth.Controllers
{
    public class SalesReportController : Controller
    {
        private readonly AppDbContext _context;

        public SalesReportController(AppDbContext context)
        {
            _context = context;
        }

        // GET: SalesReport
        public IActionResult Index()
        {
            var model = new SalesReportViewModel
            {
                FromDate = DateTime.Today.AddDays(-30), // Default to the last 30 days
                ToDate = DateTime.Today
            };
            return View(model); // Show the date picker for filtering
        }

        // POST: SalesReport/GenerateReport
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateReport(DateTime fromDate, DateTime toDate)
        {
            // Fetch sales within the selected date range
            var sales = await _context.SalesOrders
                .Where(so => so.Date >= fromDate && so.Date <= toDate)
                .Include(so => so.Product)
                .ToListAsync();

            var report = new SalesReportViewModel
            {
                FromDate = fromDate,
                ToDate = toDate,
                Sales = sales.Select(so => new SalesInfo
                {
                    ProductName = so.Product.ProductName,
                    Quantity = so.Quantity,
                    Price = so.Price,
                    Date = so.Date
                }).ToList(),
                TotalSalesValue = sales.Sum(so => so.Quantity * so.Price)
            };

            return View("ReportResult", report); // Display the report result
        }
    }
}
