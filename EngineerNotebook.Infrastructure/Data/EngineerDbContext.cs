using System.Collections.Immutable;
using System.Reflection;
using EngineerNotebook.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace EngineerNotebook.Infrastructure.Data
{
    public class EngineerDbContext : DbContext
    {
        public EngineerDbContext(DbContextOptions<EngineerDbContext> options) : base(options)
        {
        }

        public DbSet<Tag> Tags { get; set; }
        public DbSet<Documentation> Docs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}