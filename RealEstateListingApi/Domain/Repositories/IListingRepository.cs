using RealEstateListingApi.Domain.Models;

namespace RealEstateListingApi.Domain.Repositories
{
    public interface IListingRepository
    {
        Task<List<Listing>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Listing?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task AddAsync(Listing listing, CancellationToken cancellationToken = default);
        Task UpdateAsync(Listing listing, CancellationToken cancellationToken = default);
        Task DeleteAsync(Listing listing, CancellationToken cancellationToken = default);
        Task<bool> ExistsWithTitleAndAddressAsync(string title, string? address, Guid? ignoreId = null, CancellationToken cancellationToken = default);
    }
}
