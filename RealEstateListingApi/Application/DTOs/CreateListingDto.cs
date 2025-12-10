namespace RealEstateListingApi.Application.DTOs
{
    public class CreateListingDto
    {
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
    }
}
