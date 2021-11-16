using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using EngineerNotebook.Shared.Models;
using AspNetCore.Identity.Mongo.Model;
using AspNetCore.Identity.MongoDbCore.Models;

namespace EngineerNotebook.PublicApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args)
                .Build();                       
            
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();

                try
                {
                    var tagRepo = services.GetRequiredService<IAsyncRepository<Tag>>();
                    var docRepo = services.GetRequiredService<IAsyncRepository<Documentation>>();

                    await EngineerDbContextSeed.SeedAsync(tagRepo, docRepo, loggerFactory);
                    
                    var userManager = services.GetRequiredService<UserManager<ClubMember>>();
                    var roleManager = services.GetRequiredService<RoleManager<MongoIdentityRole<Guid>>>();
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
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var currentDir = Directory.GetCurrentDirectory();
                    if (!File.Exists(Path.Join(currentDir, ".env")))
                        return;

                    foreach(var line in File.ReadAllLines(Path.Join(currentDir, ".env")))
                    {
                        var parts = line.Split('=', StringSplitOptions.RemoveEmptyEntries);

                        if (parts.Length != 2)
                            continue;

                        Environment.SetEnvironmentVariable(parts[0], parts[1]);
                    }
                })
                .ConfigureWebHostDefaults(webBuilder => 
                { 
                    webBuilder.UseStartup<Startup>(); 
                });
    }
}