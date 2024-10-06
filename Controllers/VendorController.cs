// using Microsoft.AspNetCore.Mvc;
// using E_commerce_system.Data.Entities;
// using E_commerce_system.Data.Services;
// using Microsoft.AspNetCore.Authorization;

// namespace E_commerce_system.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class VendorController : ControllerBase
//     {
//         private readonly IVendorService _vendorService;

//         public VendorController(IVendorService vendorService)
//         {
//             _vendorService = vendorService;
//         }

//         [HttpGet]
//         public async Task<ActionResult<List<Vendor>>> GetAllVendors()
//         {
//             var vendors = await _vendorService.GetAllVendorsAsync();
//             return Ok(vendors);
//         }

//         [HttpGet("{id}")]
//         public async Task<ActionResult<Vendor>> GetVendor(string id)
//         {
//             var vendor = await _vendorService.GetVendorByIdAsync(id);
//             if (vendor == null)
//                 return NotFound();
//             return Ok(vendor);
//         }

//         [HttpPost]
//         [Authorize(Roles = "Administrator")]
//         public async Task<ActionResult<Vendor>> CreateVendor([FromBody] Vendor vendor)
//         {
//             var createdVendor = await _vendorService.CreateVendorAsync(vendor);
//             return CreatedAtAction(nameof(GetVendor), new { id = createdVendor.Id }, createdVendor);
//         }

//         [HttpPut("{id}")]
//         [Authorize(Roles = "Administrator")]
//         public async Task<IActionResult> UpdateVendor(string id, [FromBody] Vendor vendor)
//         {
//             var success = await _vendorService.UpdateVendorAsync(id, vendor);
//             if (!success)
//                 return NotFound();
//             return NoContent();
//         }

//         [HttpDelete("{id}")]
//         [Authorize(Roles = "Administrator")]
//         public async Task<IActionResult> DeleteVendor(string id)
//         {
//             var success = await _vendorService.DeleteVendorAsync(id);
//             if (!success)
//                 return NotFound();
//             return NoContent();
//         }

//         [HttpPost("{vendorId}/ratings")]
//         [Authorize(Roles = "Customer")]
//         public async Task<IActionResult> AddRating(string vendorId, [FromBody] VendorRating rating)
//         {
//             var success = await _vendorService.AddRatingAsync(vendorId, rating);
//             if (!success)
//                 return NotFound();
//             return Ok();
//         }

//         [HttpPut("{vendorId}/ratings/{customerId}/comment")]
//         [Authorize(Roles = "Customer")]
//         public async Task<IActionResult> UpdateComment(string vendorId, string customerId, [FromBody] string newComment)
//         {
//             var success = await _vendorService.UpdateCommentAsync(vendorId, customerId, newComment);
//             if (!success)
//                 return NotFound();
//             return NoContent();
//         }

//         [HttpGet("{vendorId}/ratings")]
//         public async Task<ActionResult<List<VendorRating>>> GetVendorRatings(string vendorId)
//         {
//             var ratings = await _vendorService.GetVendorRatingsAsync(vendorId);
//             return Ok(ratings);
//         }
//     }
// }

using Microsoft.AspNetCore.Mvc;
using E_commerce_system.Data.Entities;
using E_commerce_system.Data.Services;
using E_commerce_system.Data.DTO;
using Microsoft.AspNetCore.Authorization;

namespace E_commerce_system.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VendorController : ControllerBase
    {
        private readonly IVendorService _vendorService;

        public VendorController(IVendorService vendorService)
        {
            _vendorService = vendorService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Vendor>>> GetAllVendors()
        {
            var vendors = await _vendorService.GetAllVendorsAsync();
            return Ok(vendors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Vendor>> GetVendor(string id)
        {
            var vendor = await _vendorService.GetVendorByIdAsync(id);
            if (vendor == null)
                return NotFound();
            return Ok(vendor);
        }

        [HttpPost]
        //[Authorize(Roles = "Administrator")] // Commented out for testing
        public async Task<ActionResult<Vendor>> CreateVendor([FromBody] CreateVendorDTO vendorDto)
        {
            var vendor = new Vendor
            {
                Name = vendorDto.Name,
                Email = vendorDto.Email,
                PhoneNumber = vendorDto.PhoneNumber,
                Description = vendorDto.Description
            };

            var createdVendor = await _vendorService.CreateVendorAsync(vendor);
            return CreatedAtAction(nameof(GetVendor), new { id = createdVendor.Id }, createdVendor);
        }

        [HttpPost("{vendorId}/ratings")]
        //[Authorize(Roles = "Customer")] // Commented out for testing
        public async Task<IActionResult> AddRating(string vendorId, [FromBody] VendorRatingDTO ratingDto)
        {
            var rating = new VendorRating
            {
                CustomerId = "test-customer", // For testing purposes
                CustomerName = "Test Customer", // For testing purposes
                Rating = ratingDto.Rating,
                Comment = ratingDto.Comment
            };

            var success = await _vendorService.AddRatingAsync(vendorId, rating);
            if (!success)
                return NotFound();
            return Ok();
        }

        [HttpGet("{vendorId}/ratings")]
        public async Task<ActionResult<List<VendorRating>>> GetVendorRatings(string vendorId)
        {
            var ratings = await _vendorService.GetVendorRatingsAsync(vendorId);
            return Ok(ratings);
        }
    }
}