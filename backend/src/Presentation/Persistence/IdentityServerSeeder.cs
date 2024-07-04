using Presentation.Models;

namespace Presentation.Persistence;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading.Tasks;

public static class IdentityServerSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider, IHostEnvironment env)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (env.IsDevelopment())
            {
                await context.Database.MigrateAsync();
            }

            // Seed Roles
            var adminRole = new IdentityRole("Admin");
            if (!await roleManager.RoleExistsAsync(adminRole.Name))
            {
                await roleManager.CreateAsync(adminRole);
            }

            // Seed Users
            var adminUser = new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@example.com",
                EmailConfirmed = true
            };

            if (userManager.Users.All(u => u.UserName != adminUser.UserName))
            {
                var result = await userManager.CreateAsync(adminUser, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, adminRole.Name);
                }
            }
        }
    }
}
