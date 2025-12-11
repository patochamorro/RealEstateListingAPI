using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using RealEstateListingApi.Application.DTOs;
using RealEstateListingApi.Application.Interfaces;

namespace RealEstateListingApi.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ListingsController : ControllerBase
    {
        private readonly IListingService _listingService;
        private IValidator<ListingBaseDto> _validator;

        public ListingsController(IValidator<ListingBaseDto> validator, IListingService listingService)
        {
            _listingService = listingService;
            _validator = validator;
        }

        [HttpGet]
        [Tags("Listings Retrieval")]
        [ProducesResponseType(typeof(IEnumerable<ListingDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ListingDto>>> GetAll(CancellationToken cancellationToken)
        {
            var listings = await _listingService.GetAllAsync(cancellationToken);
            return Ok(listings);
        }

        [HttpGet("{id:guid}")]
        [Tags("Listings Retrieval")]
        [ProducesResponseType(typeof(ListingDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ListingDto>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var listing = await _listingService.GetByIdAsync(id, cancellationToken);
            if (listing is null)
            {
                return NotFound();
            }

            return Ok(listing);
        }

        /// <summary>
        /// Creates a new listing. Title+Address must be unique; returns 400 on validation or duplicate errors.
        /// </summary>
        [HttpPost]
        [Tags("Listings Management")]
        [ProducesResponseType(typeof(ListingDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ListingDto>> Create([FromBody] CreateListingDto dto, CancellationToken cancellationToken)
        {
            ValidationResult result = await _validator.ValidateAsync(dto,cancellationToken);

            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }
            var listing = await _listingService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = listing.Id }, listing);
        }

        /// <summary>
        /// Updates an existing listing by id. Title+Address must remain unique; returns 404 if not found or 400 on validation/duplicate errors.
        /// </summary>
        [HttpPut("{id:guid}")]
        [Tags("Listings Management")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateListingDto dto, CancellationToken cancellationToken)
        {
            ValidationResult result = await _validator.ValidateAsync(dto,cancellationToken);

            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }
            
            var updated = await _listingService.UpdateAsync(id, dto, cancellationToken);
            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        [Tags("Listings Management")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var deleted = await _listingService.DeleteAsync(id, cancellationToken);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
