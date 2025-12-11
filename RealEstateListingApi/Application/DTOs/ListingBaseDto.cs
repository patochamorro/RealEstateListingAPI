namespace RealEstateListingApi.Application.DTOs
{
    public abstract class ListingBaseDto
    {
        /// <summary>
        /// Listing title. Required. Max length: 20 characters.
        /// </summary>
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// Listing price. Must be greater than zero.
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// Optional description. Max length: 1000 characters.
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// Optional address. Max length: 200 characters. Combined with Title it must be unique.
        /// </summary>
        public string? Address { get; set; }
    }
}
