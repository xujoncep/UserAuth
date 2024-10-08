using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UserAuth.Data;
using UserAuth.Models;

namespace UserAuth.Controllers
{
    [Authorize(Roles = "Customer, SuperAdmin")]
    public class SalesOrderController : Controller
    {
        private readonly AppDbContext _context;

        public SalesOrderController(AppDbContext context)
        {
            _context = context;
        }

        // GET: SalesOrder
        public async Task<IActionResult> Index()
        {
            var salesOrders = await _context.SalesOrders.Include(so => so.Product).ToListAsync();
            return View(salesOrders); // Return list of sales orders
        }

        // GET: SalesOrder/AddNewSales
        public IActionResult AddNewSales()
        {
            ViewBag.Products = new SelectList(_context.Products, "ProductId", "ProductName");
            return PartialView(); // Return partial view for the modal
        }

        // POST: SalesOrder/AddNewSales
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNewSales(SalesOrder salesOrder)
        {
            if (!ModelState.IsValid)
            {
                // Fetch the product details for the selected product
                var product = await _context.Products.FindAsync(salesOrder.ProductId);
                if (product == null || product.Quantity < salesOrder.Quantity)
                {
                    ModelState.AddModelError("", "Insufficient stock.");
                    ViewBag.Products = new SelectList(_context.Products, "ProductId", "ProductName", salesOrder.ProductId);
                    return View(salesOrder);
                }

                // Set the price of the sales order from the product price
                salesOrder.Price = product.Price;

                // Add the sales order to the database
                _context.SalesOrders.Add(salesOrder);

                // Reduce product quantity
                product.Quantity -= salesOrder.Quantity;

                // Save changes
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Products = new SelectList(_context.Products, "ProductId", "ProductName", salesOrder.ProductId);
            return View(salesOrder);
        }
    }
}
