using AspNetCore.Identity.Mongo.Model;
using AspNetCore.Identity.MongoDbCore.Models;
using EngineerNotebook.Core.Constants;
using EngineerNotebook.Shared.Authorization;
using EngineerNotebook.Shared.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace EngineerNotebook.Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedAsync(UserManager<ClubMember> userManager,
            RoleManager<MongoIdentityRole<Guid>> roleManager)
        {
            #region Add the necessary roles for our application
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new MongoIdentityRole<Guid>(Roles.ADMINISTRATORS));
            }
            #endregion

            #region Admin Account
            if(!userManager.Users.Any())
            {
                string adminUserName = "admin@ddc.org";
                var adminUser = new ClubMember { UserName = adminUserName, Email = adminUserName };
                await userManager.CreateAsync(adminUser, AuthorizationConstants.DEFAULT_PASSWORD);
                adminUser = await userManager.FindByNameAsync(adminUserName);
                await userManager.AddToRoleAsync(adminUser, Roles.ADMINISTRATORS);
            }
            #endregion
        }
    }
}