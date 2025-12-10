using Moq;
using RealEstateListingApi.Application.DTOs;
using RealEstateListingApi.Application.Interfaces;
using RealEstateListingApi.Application.Services;
using RealEstateListingApi.Domain.Models;
using RealEstateListingApi.Domain.Repositories;

namespace RealEstateListingApi.Tests.Application.Services;

public class ListingServiceTests
{
    private readonly Mock<IListingRepository> _repositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly ListingService _service;

    public ListingServiceTests()
    {
        _service = new ListingService(_repositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsDtos()
    {
        // Arrange
        var listings = new List<Listing>
        {
            new() { Id = Guid.NewGuid(), Title = "A", Price = 100, Description = "desc", Address = "addr" },
            new() { Id = Guid.NewGuid(), Title = "B", Price = 200, Description = "desc2", Address = "addr2" }
        };
        _repositoryMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(listings);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Contains(result, r => r.Title == "A" && r.Price == 100);
        Assert.Contains(result, r => r.Title == "B" && r.Price == 200);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
    {
        _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Listing?)null);

        var result = await _service.GetByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_AddsListing_AndSaves()
    {
        // Arrange
        var dto = new CreateListingDto
        {
            Title = "New",
            Price = 123.45m,
            Description = "Desc",
            Address = "Addr"
        };

        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Listing>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert
        _repositoryMock.Verify(r => r.AddAsync(It.Is<Listing>(l =>
            l.Title == dto.Title &&
            l.Price == dto.Price &&
            l.Description == dto.Description &&
            l.Address == dto.Address), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        Assert.Equal(dto.Title, result.Title);
        Assert.Equal(dto.Price, result.Price);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsFalse_WhenListingMissing()
    {
        _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Listing?)null);

        var updated = await _service.UpdateAsync(Guid.NewGuid(), new UpdateListingDto(), CancellationToken.None);

        Assert.False(updated);
        _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Listing>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsTrue_WhenUpdated()
    {
        // Arrange
        var id = Guid.NewGuid();
        var listing = new Listing { Id = id, Title = "Old", Price = 1, Description = "d", Address = "a" };
        var dto = new UpdateListingDto { Title = "New", Price = 2, Description = "d2", Address = "a2" };

        _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(listing);
        _repositoryMock.Setup(r => r.UpdateAsync(listing, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        var updated = await _service.UpdateAsync(id, dto, CancellationToken.None);

        // Assert
        Assert.True(updated);
        Assert.Equal(dto.Title, listing.Title);
        Assert.Equal(dto.Price, listing.Price);
        Assert.Equal(dto.Description, listing.Description);
        Assert.Equal(dto.Address, listing.Address);
        _repositoryMock.Verify(r => r.UpdateAsync(listing, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalse_WhenListingMissing()
    {
        _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Listing?)null);

        var deleted = await _service.DeleteAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.False(deleted);
        _repositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Listing>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsTrue_WhenDeleted()
    {
        var listing = new Listing { Id = Guid.NewGuid(), Title = "T", Price = 1, Description = "d", Address = "a" };
        _repositoryMock.Setup(r => r.GetByIdAsync(listing.Id, It.IsAny<CancellationToken>())).ReturnsAsync(listing);
        _repositoryMock.Setup(r => r.DeleteAsync(listing, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var deleted = await _service.DeleteAsync(listing.Id, CancellationToken.None);

        Assert.True(deleted);
        _repositoryMock.Verify(r => r.DeleteAsync(listing, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
