using System.Net;
using System.Net.Http.Json;
using RealEstateListingApi.Application.DTOs;

namespace RealEstateListingApi.Tests.Integration;

public class ListingsControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ListingsControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ReturnsEmptyList_Initially()
    {
        var response = await _client.GetAsync("/api/Listings");
        response.EnsureSuccessStatusCode();

        var listings = await response.Content.ReadFromJsonAsync<IEnumerable<ListingDto>>();
        Assert.NotNull(listings);
        Assert.Empty(listings!);
    }

    [Fact]
    public async Task Create_Then_GetById_ReturnsCreatedListing()
    {
        // Arrange
        var createDto = new CreateListingDto
        {
            Title = "Integtioran Test Home",
            Price = 500000,
            Description = "Nice place",
            Address = "123 Test Street"
        };

        // Act - create
        var createResponse = await _client.PostAsJsonAsync("/api/Listings", createDto);

        // Assert - creation
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
        var created = await createResponse.Content.ReadFromJsonAsync<ListingDto>();
        Assert.NotNull(created);
        Assert.Equal(createDto.Title, created!.Title);
        Assert.NotEqual(Guid.Empty, created.Id);

        // Act - get by id
        var getResponse = await _client.GetAsync($"/api/Listings/{created.Id}");
        getResponse.EnsureSuccessStatusCode();

        var fetched = await getResponse.Content.ReadFromJsonAsync<ListingDto>();
        Assert.NotNull(fetched);
        Assert.Equal(created.Id, fetched!.Id);
        Assert.Equal(createDto.Title, fetched.Title);
        Assert.Equal(createDto.Price, fetched.Price);
    }
}
