using System.Threading.Tasks;
using EngineerNotebook.Core.Constants;
using EngineerNotebook.Shared.Authorization;
using Microsoft.AspNetCore.Identity;

namespace EngineerNotebook.Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            #region Add the necessary roles for our application
            await roleManager.CreateAsync(new IdentityRole(Roles.ADMINISTRATORS));
            await roleManager.CreateAsync(new IdentityRole(Roles.CONTRIBUTORS));
            #endregion

            #region Admin Account
            string adminUserName = "admin@ddc.org";
            var adminUser = new ApplicationUser { UserName = adminUserName, Email = adminUserName };
            await userManager.CreateAsync(adminUser, AuthorizationConstants.DEFAULT_PASSWORD);
            adminUser = await userManager.FindByNameAsync(adminUserName);
            await userManager.AddToRoleAsync(adminUser, Roles.ADMINISTRATORS);
            #endregion
        }
    }
}