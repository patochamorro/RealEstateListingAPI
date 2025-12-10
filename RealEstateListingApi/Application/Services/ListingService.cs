using RealEstateListingApi.Application.DTOs;
using RealEstateListingApi.Application.Interfaces;
using RealEstateListingApi.Domain.Models;
using RealEstateListingApi.Domain.Repositories;

namespace RealEstateListingApi.Application.Services
{
    public class ListingService : IListingService
    {
        private readonly IListingRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ListingService(IListingRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ListingDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var listings = await _repository.GetAllAsync(cancellationToken);
            return listings.Select(MapToDto);
        }

        public async Task<ListingDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var listing = await _repository.GetByIdAsync(id, cancellationToken);
            return listing is null ? null : MapToDto(listing);
        }

        public async Task<ListingDto> CreateAsync(CreateListingDto dto, CancellationToken cancellationToken = default)
        {
            var listing = new Listing
            {
                Title = dto.Title,
                Price = dto.Price,
                Description = dto.Description,
                Address = dto.Address
            };

            await _repository.AddAsync(listing, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return MapToDto(listing);
        }

        public async Task<bool> UpdateAsync(Guid id, UpdateListingDto dto, CancellationToken cancellationToken = default)
        {
            var listing = await _repository.GetByIdAsync(id, cancellationToken);
            if (listing is null)
            {
                return false;
            }

            listing.Title = dto.Title;
            listing.Price = dto.Price;
            listing.Description = dto.Description;
            listing.Address = dto.Address;

            await _repository.UpdateAsync(listing, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var listing = await _repository.GetByIdAsync(id, cancellationToken);
            if (listing is null)
            {
                return false;
            }

            await _repository.DeleteAsync(listing, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        private static ListingDto MapToDto(Listing listing) =>
            new()
            {
                Id = listing.Id,
                Title = listing.Title,
                Price = listing.Price,
                Description = listing.Description,
                Address = listing.Address
            };
    }
}
