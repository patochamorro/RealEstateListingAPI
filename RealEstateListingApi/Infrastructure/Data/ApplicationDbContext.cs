using Microsoft.EntityFrameworkCore;
using RealEstateListingApi.Domain.Models;

namespace RealEstateListingApi.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Listing> Listings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Listing>(entity =>
            {
                entity.Property(l => l.Title)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(l => l.Price)
                      .HasColumnType("decimal(18,2)");

                entity.Property(l => l.Description)
                      .HasMaxLength(2000);

                entity.Property(l => l.Address)
                      .HasMaxLength(500);
            });
        }
    }
}
