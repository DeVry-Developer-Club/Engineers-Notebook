using System;
using System.Threading.Tasks;
using EngineerNotebook.Infrastructure.Data;
using EngineerNotebook.Infrastructure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EngineerNotebook.PublicApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var hostBuilder = CreateHostBuilder(args);
            
            #if RELEASE
            hostBuilder = hostBuilder.UseEnvironment("Production");
            #endif
            
            var host = hostBuilder.Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();

                try
                {
                    var context = services.GetRequiredService<EngineerDbContext>();
                    await EngineerDbContextSeed.SeedAsync(context, loggerFactory);
                    
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    await AppIdentityDbContextSeed.SeedAsync(userManager, roleManager);
                }
                catch (Exception ex)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(ex, "An error occurred seeding the DB");
                }
            }
            
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}