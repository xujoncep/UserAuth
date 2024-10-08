using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UserAuth.Models;
using UserAuth.ViewModels;

namespace UserAuth.Controllers
{
    [Authorize(Roles = "Admin , SuperAdmin")]
    public class UserManagementController : Controller
    {
        private readonly UserManager<Users> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserManagementController(UserManager<Users> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: UserManagement
        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();
            return View(users); // Return the list of users to the view
        }

        // GET: UserManagement/AssignRole/5
        public async Task<IActionResult> AssignRole(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _roleManager.Roles.ToListAsync();
            var model = new AssignRoleViewModel
            {
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Roles = new SelectList(roles, "Name", "Name")
            };

            return PartialView(model); // Return partial view with user and role info
        }

        // POST: UserManagement/AssignRole
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignRole(AssignRoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user == null)
                {
                    return NotFound();
                }

                // Remove old roles and assign new role
                var roles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, roles);

                var result = await _userManager.AddToRoleAsync(user, model.Role);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(StaffList)); // Redirect after success
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            var rolesList = await _roleManager.Roles.ToListAsync();
            model.Roles = new SelectList(rolesList, "Name", "Name");
            return View(model); // Return view with errors if any
        }

        // GET: UserManagement/StaffList
        public async Task<IActionResult> StaffList()
        {
            var users = _userManager.Users.ToList();
            var model = new List<StaffListViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                model.Add(new StaffListViewModel
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Role = roles.FirstOrDefault()
                });
            }

            return View(model); // Return view with staff list
        }
    }
}
