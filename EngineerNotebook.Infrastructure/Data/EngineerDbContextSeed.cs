using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EngineerNotebook.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EngineerNotebook.Infrastructure.Data
{
    public class EngineerDbContextSeed
    {
        public static async Task SeedAsync(EngineerDbContext context, ILoggerFactory loggerFactory, int retry = 0)
        {
            int retryForAvailability = retry;

            try
            {
                // SEED TAGS --> IF NONE EXIST ALREADY
                if (!await context.Tags.AnyAsync())
                {
                    await context.Tags.AddRangeAsync(GetPreconfiguredTags());
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if (retryForAvailability < 10)
                {
                    retryForAvailability++;
                    var log = loggerFactory.CreateLogger<EngineerDbContextSeed>();
                    log.LogError(ex.Message);
                    await SeedAsync(context, loggerFactory, retryForAvailability);
                }

                throw;
            }
        }

        static IEnumerable<Tag> GetPreconfiguredTags()
        {
            return new List<Tag>
            {
                new() { Name = "Ai" },
                new() { Name = "Azure" },
                new() { Name = "C#" },
                new() { Name = "Classes" },
                new() { Name = "Club Roadmap", TagType = TagType.Phrase },
                new() { Name = "DevOps" },
                new() { Name = "Docker" },
                new() { Name = "Exception" },
                new() { Name = "Function"},
                new() { Name = "Git" },
                new() { Name = "Github" },
                new() { Name = "Install", TagType = TagType.Prefix },
                new() { Name = "Java" },
                new() { Name = "Kubernetes" },
                new() { Name = "Mongo" },                
                new() { Name = "MySql" },
                new() { Name = "Program", TagType = TagType.Prefix },
                new() { Name = "Python" },
                new() { Name = "RobotWars" },
                new() { Name = "Rust" },
                new() { Name = "SQL" },
                new() { Name = "Use", TagType = TagType.Prefix }
            };
        }
    }
}