using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UserAuth.Data;
using UserAuth.Models;

namespace UserAuth.Controllers
{
    [Authorize(Roles = "Manager, SuperAdmin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Product
        public IActionResult Index()
        {
            var products = _context.Products.Include(p => p.Category).ToList();
            return View(products);
        }

        // GET: Product/AddOrEdit
        public IActionResult AddOrEdit(int? id)
        {
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "CategoryName");

            if (id == null || id == 0)
            {
                return PartialView(new Product()); // Add new product
            }
            else
            {
                var product = _context.Products.Find(id);
                if (product == null)
                {
                    return NotFound();
                }
                return PartialView(product); // Edit existing product
            }
        }

        // POST: Product/AddOrEdit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOrEdit(Product product)
        {
            if (!ModelState.IsValid)
            {
                if (product.ProductId == 0) // New Product
                {
                    _context.Products.Add(product);
                }
                else // Existing Product
                {
                    _context.Products.Update(product);
                }
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            // If the model state is invalid, reload the form with errors
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        // GET: Product/Delete
        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
