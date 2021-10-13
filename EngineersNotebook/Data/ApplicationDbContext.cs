using System;
using System.Collections.Generic;
using System.Text;
using EngineersNotebook.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EngineersNotebook.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Documentation>()
                .HasMany(x => x.Tags)
                .WithMany(x => x.Docs);
            
        }

        public DbSet<Documentation> Docs { get; set; }
        public DbSet<Tag> Tags { get; set; }
    }
}