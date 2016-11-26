using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace GitGud.Models
{
    public class SeedAdmin
    {
        private GitGudContext _context;
        private RoleManager<Role> _roleManager;

        public SeedAdmin(GitGudContext context, RoleManager<Role> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }

        public async void SeedRoles()
        {
            if (!_roleManager.RoleExistsAsync("User").Result)
            {
                Role role = new Role();
                role.Name = "User";
                role.Description = "Perform normal operations.";
                IdentityResult roleResult = _roleManager.CreateAsync(role).Result;
            }

            if (!_roleManager.RoleExistsAsync("Admin").Result)
            {
                Role role = new Role();
                role.Name = "Admin";
                role.Description = "Manage everything";
                IdentityResult roleResult = _roleManager.CreateAsync(role).Result;
            }

//            if (!_context.Users.Any(u => u.UserName == "Admin"))
//            {
//                var user = new User
//                {
//                    UserName = "Admin",
//                    Email = "admin@test.bg",
//                    NormalizedEmail = "admin@test.bg",
//                    NormalizedUserName = "Admin",
//                    LockoutEnabled = true,
//                    SecurityStamp = Guid.NewGuid().ToString()
//                };
//
//                var password = new PasswordHasher<User>();
//                var hashed = password.HashPassword(user, "admin");
//                user.PasswordHash = hashed;
//                var userStore = new UserStore<User>(_context);
//                await userStore.CreateAsync(user);
//                await userStore.AddToRoleAsync(user, "admin");
//            }

            await _context.SaveChangesAsync();
        }
    }
}