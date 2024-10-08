using Microsoft.AspNetCore.Mvc.Rendering;

namespace UserAuth.ViewModels
{
    public class AssignRoleViewModel
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; } // Role selected by the user
        public SelectList Roles { get; set; } // List of available roles
    }
}
