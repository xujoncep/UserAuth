using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserAuth.Data;
using UserAuth.ViewModels;

namespace UserAuth.Controllers
{
    public class PurchaseReportController : Controller
    {
        private readonly AppDbContext _context;

        public PurchaseReportController(AppDbContext context)
        {
            _context = context;
        }

        // GET: PurchaseReport
        public IActionResult Index()
        {
            var model = new PurchaseReportViewModel
            {
                FromDate = DateTime.Today.AddDays(-30), // Default to the last 30 days
                ToDate = DateTime.Today
            };
            return View(model); // Show the date picker for filtering
        }

        // POST: PurchaseReport/GenerateReport
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateReport(DateTime fromDate, DateTime toDate)
        {
            // Fetch purchases within the selected date range
            var purchases = await _context.PurchaseOrders
                .Where(po => po.Date >= fromDate && po.Date <= toDate)
                .Include(po => po.Product)
                .ToListAsync();

            var report = new PurchaseReportViewModel
            {
                FromDate = fromDate,
                ToDate = toDate,
                Purchases = purchases.Select(po => new PurchaseInfo
                {
                    ProductName = po.Product.ProductName,
                    Quantity = po.Quantity,
                    Price = po.Price,
                    Date = po.Date
                }).ToList(),
                TotalPurchaseValue = purchases.Sum(po => po.Quantity * po.Price)
            };

            return View("ReportResult", report); // Display the report result
        }
    }
}
