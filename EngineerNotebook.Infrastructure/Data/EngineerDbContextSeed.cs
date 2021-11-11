using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EngineerNotebook.Core.Interfaces;
using EngineerNotebook.Shared.Extensions;
using EngineerNotebook.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EngineerNotebook.Infrastructure.Data
{
    public class EngineerDbContextSeed
    {
        public static async Task SeedAsync(IAsyncRepository<Shared.Models.Tag> tagRepo,
            IAsyncRepository<Documentation> docRepo, 
            ILoggerFactory loggerFactory, 
            int retry = 0)
        {
            int retryForAvailability = retry;
            
            try
            {
                // SEED TAGS --> IF NONE EXIST ALREADY
                if (!await tagRepo.AnyAsync())
                    foreach (var tag in GetPreconfiguredTags())
                        await tagRepo.Create(tag);
                
                // SEED DOCS --> IF NONE EXIST ALREADY
                if (!await docRepo.AnyAsync())
                    foreach (var doc in GetPreconfiguredDocs())
                        await docRepo.Create(doc);
                
            }
            catch (Exception ex)
            {
                if (retryForAvailability < 10)
                {
                    retryForAvailability++;
                    var log = loggerFactory.CreateLogger<EngineerDbContextSeed>();
                    log.LogError(ex.Message);
                    await SeedAsync(tagRepo, docRepo, loggerFactory, retryForAvailability);
                }

                throw;
            }
        }

        static IEnumerable<Documentation> GetPreconfiguredDocs()
        {
            const string admin = "admin@ddc.org";
            
            return new List<Documentation>()
            {
                new()
                {
                    Title = "Python",
                    Description = "Super awesome description here",
                    Contents = "<h2>Header</h2><p>something cool here</p>".ToBase64(),
                    CreatedByUserId = admin
                },
                
                new()
                {
                    Title = "Sample 2",
                    Description = "This is description for second item",
                    Contents = "<h2>Cool</h2><p>I am a banana</p>".ToBase64(),
                    CreatedByUserId = admin
                }
            };
        }

        static IEnumerable<Shared.Models.Tag> GetPreconfiguredTags()
        {
            return new List<Shared.Models.Tag>
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