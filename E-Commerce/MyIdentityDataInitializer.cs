using Microsoft.AspNetCore.Identity;

namespace E_Commerce
{
    public class MyIdentityDataInitializer
    {
        public static async Task SeedData(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
         await SeedRoles(roleManager);
         await SeedUsers(userManager);
        }

        public static async Task SeedUsers(
           UserManager<IdentityUser> userManager)
        {
            if (await userManager.FindByNameAsync("admin@website.com") == null)
            {
                var user = new IdentityUser
                {
                    UserName = "admin@website.com",
                    Email = "admin@website.com",
                    
                };
                var result = await userManager.CreateAsync(user, "a@1234567");
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }
        }

        public static async Task SeedRoles(
            RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                var role = new IdentityRole
                {
                    Name = "Admin",
                };
                var result = await roleManager.CreateAsync(role);
            }

        }
    }
}
