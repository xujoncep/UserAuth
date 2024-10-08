using Microsoft.AspNetCore.Mvc;
using UserAuth.Data;
using UserAuth.Models;

namespace UserAuth.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Category
        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }

        // GET: Category/AddOrEdit
        public IActionResult AddOrEdit(int? id)
        {
            if (id == null || id == 0)
            {
                return PartialView(new Category()); // Empty form for Add
            }
            else
            {
                var category = _context.Categories.Find(id);
                if (category == null)
                {
                    return NotFound();
                }
                return PartialView(category); // Filled form for Edit
            }
        }

        // POST: Category/AddOrEdit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOrEdit(Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.CategoryId == 0)
                {
                    _context.Categories.Add(category);
                }
                else
                {
                    _context.Categories.Update(category);
                }
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Category/Delete
        public IActionResult Delete(int id)
        {
            var category = _context.Categories.Find(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
