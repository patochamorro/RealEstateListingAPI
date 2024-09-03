using Microsoft.EntityFrameworkCore;
using RealEstateListingApi.Models;

namespace RealEstateListingApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Listing> Listings { get; set; }
    }
}
