namespace RealEstateListingApi.Models
{
    public class Listing
    {
    public string Id { get; set; } = string.Empty;  // Default to empty string if nulls aren't allowed
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }  // Decimal is a value type and non-nullable by default
    public string? Description { get; set; }  // Mark as nullable if appropriate
}
}