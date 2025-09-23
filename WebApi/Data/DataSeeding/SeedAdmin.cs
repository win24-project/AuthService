using Microsoft.AspNetCore.Identity;
using WebApi.Data.Entities;

namespace WebApi.Data.DataSeeding;

public static class SeedAdmin
{
    public static async Task EnsureAdminUserExistAsync(IServiceProvider serviceProvider, IConfiguration config)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<UserEntity>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        string adminEmail = config["AdminEmail"];
        string adminPassword = config["AdminPassword"]; 
        string adminRole = "Admin";

        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new UserEntity
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, adminRole);
            }
        }
        else
        {
            if (!await userManager.IsInRoleAsync(adminUser, adminRole))
            {
                await userManager.AddToRoleAsync(adminUser, adminRole);
            }
        }
    }
}
