﻿using Microsoft.AspNetCore.Identity;

namespace UserAuth.Models
{
    public class Users : IdentityUser
    {
        public string FullName { get; set; }
    }
}
