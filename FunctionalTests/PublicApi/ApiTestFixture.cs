using System;
using EngineerNotebook.Infrastructure.Data;
using EngineerNotebook.Infrastructure.Identity;
using EngineerNotebook.PublicApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FunctionalTests.PublicApi
{
    public class ApiTestFixture : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                services.AddEntityFrameworkInMemoryDatabase();

                // create a new services provider.
                var provider = services.AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                // add a database context using an in-memory db for testing
                services.AddDbContext<EngineerDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                    options.UseInternalServiceProvider(provider);
                });

                services.AddDbContext<AppIdentityDbContext>(options =>
                {
                    options.UseInMemoryDatabase("Identity");
                    options.UseInternalServiceProvider(provider);
                });

                // build the service provider
                var sp = services.BuildServiceProvider();

                // create a scope to obtain a reference to the database
                // context (EngineerDbContext)
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<EngineerDbContext>();
                    var loggerFactory = scopedServices.GetRequiredService<ILoggerFactory>();

                    var logger = scopedServices.GetRequiredService<ILogger<ApiTestFixture>>();

                    // ensure the database is created
                    db.Database.EnsureCreated();

                    // seed the database with test data
                    try
                    {
                        EngineerDbContextSeed.SeedAsync(db, loggerFactory).Wait();

                        // seed sample user data
                        var userManager = scopedServices.GetRequiredService<UserManager<ApplicationUser>>();
                        var roleManager = scopedServices.GetRequiredService<RoleManager<IdentityRole>>();

                        AppIdentityDbContextSeed.SeedAsync(userManager, roleManager).Wait();
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, $"An error occurred seeding the database with " +
                                            $"test data. Error: {ex.Message}");
                    }
                }
            });
        }
    }
}