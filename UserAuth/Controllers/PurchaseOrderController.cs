using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UserAuth.Data;
using UserAuth.Models;

namespace UserAuth.Controllers
{
    public class PurchaseOrderController : Controller
    {
        private readonly AppDbContext _context;

        public PurchaseOrderController(AppDbContext context)
        {
            _context = context;
        }

        // GET: PurchaseOrder
        public async Task<IActionResult> Index()
        {
            var purchaseOrders = await _context.PurchaseOrders.Include(po => po.Product).ToListAsync();
            return View(purchaseOrders); // Return list of purchase orders
        }

        // GET: PurchaseOrder/AddNewPurchase
        public IActionResult AddNewPurchase()
        {
            ViewBag.Products = new SelectList(_context.Products, "ProductId", "ProductName");
            return PartialView(); // Return partial view for the modal
        }

        // POST: PurchaseOrder/AddNewPurchase
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNewPurchase(PurchaseOrder purchaseOrder)
        {
            if (!ModelState.IsValid)
            {
                // Fetch the product details for the selected product
                var product = await _context.Products.FindAsync(purchaseOrder.ProductId);
                if (product == null)
                {
                    return NotFound();
                }

                // Set the price of the purchase order from the product price
                purchaseOrder.Price = product.Price;

                // Add the purchase order to the database
                _context.PurchaseOrders.Add(purchaseOrder);

                // Update product quantity
                product.Quantity += purchaseOrder.Quantity;

                // Save changes
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Products = new SelectList(_context.Products, "ProductId", "ProductName", purchaseOrder.ProductId);
            return View(purchaseOrder);
        }
    }
}
