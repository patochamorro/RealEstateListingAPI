using RealEstateListingApi.Application.DTOs;

namespace RealEstateListingApi.Application.Interfaces
{
    public interface IListingService
    {
        Task<IEnumerable<ListingDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ListingDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<ListingDto> CreateAsync(CreateListingDto dto, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(Guid id, UpdateListingDto dto, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
