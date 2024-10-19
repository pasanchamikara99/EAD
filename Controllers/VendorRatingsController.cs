using Microsoft.AspNetCore.Mvc;
using E_commerce_system.Data;
using E_commerce_system.Entities;
using MongoDB.Driver;

namespace E_commerce_system.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VendorRatingsController : ControllerBase
    {
        private readonly IMongoCollection<VendorRatings>? _vendorRatings;

        public VendorRatingsController(MongoDbService mongoDbService)
        {
            _vendorRatings = mongoDbService.Database?.GetCollection<VendorRatings>("vendorRatings");
        }

        // Get all Vendor Ratings
        [HttpGet]
        public async Task<IEnumerable<VendorRatings>> Get()
        {
            return await _vendorRatings.Find(FilterDefinition<VendorRatings>.Empty).ToListAsync();
        }

        // Get Vendor Rating by Id
        [HttpGet("{id}")]
        public async Task<ActionResult<VendorRatings?>> GetById(string id)
        {
            var filter = Builders<VendorRatings>.Filter.Eq(x => x.Id, id);
            var vendorRating = await _vendorRatings.Find(filter).FirstOrDefaultAsync();
            return vendorRating is not null ? Ok(vendorRating) : NotFound();
        }

        // Create Vendor Rating
        [HttpPost]
        public async Task<ActionResult> Post(VendorRatings vendorRating)
        {
            if (vendorRating == null)
            {
                return BadRequest("Vendor rating is null.");
            }

            await _vendorRatings.InsertOneAsync(vendorRating);
            return CreatedAtAction(nameof(GetById), new { id = vendorRating.Id }, vendorRating);
        }


        // Get Vendor Ratings by Vendor Id
        [HttpGet("getVendorRatingsByVendorId/{vendorId}")]
        public async Task<IEnumerable<VendorRatings>> GetVendorRatingsByVendorId(string vendorId)
        {
            var filter = Builders<VendorRatings>.Filter.Eq(x => x.VendorId, vendorId);
            return await _vendorRatings.Find(filter).ToListAsync();
        }

    }
}
