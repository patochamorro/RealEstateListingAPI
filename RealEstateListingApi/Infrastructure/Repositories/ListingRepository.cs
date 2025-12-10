using Microsoft.EntityFrameworkCore;
using RealEstateListingApi.Domain.Models;
using RealEstateListingApi.Domain.Repositories;
using RealEstateListingApi.Infrastructure.Data;

namespace RealEstateListingApi.Infrastructure.Repositories
{
    public class ListingRepository : IListingRepository
    {
        private readonly ApplicationDbContext _context;

        public ListingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Listing>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Listings.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<Listing?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Listings.AsNoTracking().FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
        }

        public async Task AddAsync(Listing listing, CancellationToken cancellationToken = default)
        {
            await _context.Listings.AddAsync(listing, cancellationToken);
        }

        public Task UpdateAsync(Listing listing, CancellationToken cancellationToken = default)
        {
            _context.Listings.Update(listing);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Listing listing, CancellationToken cancellationToken = default)
        {
            _context.Listings.Remove(listing);
            return Task.CompletedTask;
        }
    }
}
