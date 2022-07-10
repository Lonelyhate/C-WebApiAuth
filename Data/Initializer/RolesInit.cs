using apiLeran.Models;
using Microsoft.AspNetCore.Identity;

namespace apiLeran.Data.Initializer;

public class RolesInit
{
    public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        string adminEmail = "admin@mail.ru";
        string password = "123456";
        if (await roleManager.FindByNameAsync("admin") == null)
            await roleManager.CreateAsync(new IdentityRole("admin"));
        if (await roleManager.FindByNameAsync("employee") == null)
            await roleManager.CreateAsync(new IdentityRole("employee"));
        if (await roleManager.FindByNameAsync("user") == null)
            await roleManager.CreateAsync(new IdentityRole("user"));

        if (await roleManager.FindByNameAsync(adminEmail) == null)
        {
            User admin = new User { Email = adminEmail, UserName = adminEmail };
            IdentityResult result = await userManager.CreateAsync(admin, password);
            if (result.Succeeded)
            {
                admin.EmailConfirmed = true;
                admin.Name = "Никита админ";
                await userManager.AddToRoleAsync(admin, "admin");
            }
        }
    }
}