using BlazorApp1.Core.Entities;
using BlazorApp1.Infrastructure.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace BlazorApp1.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Customer> Customers { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Customer>(b =>
            {
                b.HasKey(c => c.Id);
                b.Property(c => c.FirstName).HasMaxLength(100).IsRequired();
                b.Property(c => c.LastName).HasMaxLength(100).IsRequired();
                b.Property(c => c.Email).HasMaxLength(256).IsRequired();
                b.Property(c => c.DateOfBirth).HasColumnType("date");
            });
        }
    }
}
