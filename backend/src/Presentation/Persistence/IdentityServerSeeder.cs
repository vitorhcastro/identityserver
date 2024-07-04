namespace Presentation.Persistence;

using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;

public static class IdentityServerSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider, IHostEnvironment env)
    {
        using var serviceScope = serviceProvider.CreateScope();
        var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var configurationDbContext = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
        var persistedGrantDbContext = serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>();
        var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        if (env.IsDevelopment())
        {
            await context.Database.MigrateAsync();
            await configurationDbContext.Database.MigrateAsync();
            await persistedGrantDbContext.Database.MigrateAsync();
        }

        // Seed Roles
        var adminRole = new IdentityRole("Admin");
        if (!await roleManager.RoleExistsAsync(adminRole.Name!))
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
            var result = await userManager.CreateAsync(adminUser, "Pass123."); // Admin password
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, adminRole.Name!);
            }
        }

        // Seed Identity Resources
        if (!configurationDbContext.IdentityResources.Any())
        {
            foreach (var resource in new IdentityResource[]
                     {
                         new IdentityResources.OpenId(),
                         new IdentityResources.Profile()
                     })
            {
                configurationDbContext.IdentityResources.Add(resource.ToEntity());
            }

            await configurationDbContext.SaveChangesAsync();
        }

        // Seed API Scopes
        if (!configurationDbContext.ApiScopes.Any())
        {
            foreach (var scope in new ApiScope[]
                     {
                         new("identity-server-api", "My API")
                     })
            {
                configurationDbContext.ApiScopes.Add(scope.ToEntity());
            }

            await configurationDbContext.SaveChangesAsync();
        }

        // Seed API Resources
        if (!configurationDbContext.ApiResources.Any())
        {
            foreach (var resource in new ApiResource[]
                     {
                         new("identity-server-api", "My API")
                         {
                             Scopes = { "identity-server-api" }
                         }
                     })
            {
                configurationDbContext.ApiResources.Add(resource.ToEntity());
            }

            await configurationDbContext.SaveChangesAsync();
        }

        // Seed Clients
        if (!configurationDbContext.Clients.Any())
        {
            foreach (var client in new Client[]
                     {
                         new()
                         {
                             ClientId = "react-client",
                             ClientName = "React Client",
                             AllowedGrantTypes = GrantTypes.Code,
                             RequirePkce = true,
                             RequireClientSecret = false,
                             RedirectUris = { "http://localhost:3000/callback" },
                             PostLogoutRedirectUris = { "http://localhost:3000/" },
                             AllowedCorsOrigins = { "http://localhost:3000" },
                             AllowedScopes = { "openid", "profile", "identity-server-api" },
                             AllowAccessTokensViaBrowser = true,
                             RequireConsent = false
                         },
                         new()
                         {
                             ClientId = "mvc",
                             ClientName = "MVC Client",
                             AllowedGrantTypes = GrantTypes.Code,
                             RequirePkce = true,
                             ClientSecrets = { new Secret("secret".Sha256()) },
                             RedirectUris = { "https://localhost:7196/signin-oidc" },
                             PostLogoutRedirectUris = { "https://localhost:7196/signout-callback-oidc" },
                             AllowedScopes = { "openid", "profile", "identity-server-api" },
                             AllowAccessTokensViaBrowser = true,
                             RequireConsent = false
                         }
                     })
            {
                configurationDbContext.Clients.Add(client.ToEntity());
            }

            await configurationDbContext.SaveChangesAsync();
        }
    }
}