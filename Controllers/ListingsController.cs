using Microsoft.AspNetCore.Mvc;
using RealEstateListingApi.Data;
using RealEstateListingApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace RealEstateListingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ListingsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ListingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Tag this operation as "Listings Retrieval"
        [HttpGet]
        [Tags("Listings Retrieval")]
        public ActionResult<IEnumerable<Listing>> GetAllListings()
        {
            return _context.Listings.ToList();
        }

        // Tag this operation as "Listings Management"
        [HttpPost]
        [Tags("Listings Management")]
        public ActionResult<Listing> AddListing([FromBody] Listing listing)
        {
            _context.Listings.Add(listing);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetListingById), new { id = listing.Id }, listing);
        }

        // Tag this operation as "Listings Retrieval"
        [HttpGet("{id}")]
        [Tags("Listings Retrieval")]
        public ActionResult<Listing> GetListingById(string id)
        {
            var listing = _context.Listings.FirstOrDefault(l => l.Id == id);
            if (listing == null)
            {
                return NotFound();
            }
            return listing;
        }
    }
}
