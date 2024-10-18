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
using E_commerce_system.Data;
using Microsoft.AspNetCore.Authorization;
using E_commerce_system.DTO;

/*
 * File: OrderController.cs
 * Author: Amisha Prathyanga
 * Purpose: Manages order operations including creating, retrieving, updating, canceling, marking as delivered or dispatched, 
 *          and viewing orders by user or vendor. This controller facilitates the interaction between the client-side 
 *          and the underlying order management services, ensuring smooth order processing and tracking.
 */
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

        // Method: GetAllVendors
        // Purpose: Retrieves a list of all vendors from the database.
        // Returns: A list of Vendor entities.
        [HttpGet]
        public async Task<ActionResult<List<Vendor>>> GetAllVendors()
        {
            var vendors = await _vendorService.GetAllVendorsAsync();
            return Ok(vendors);
        }

        // Method: GetVendor
        // Purpose: Retrieves a specific vendor by ID.
        // Parameters: id - The ID of the vendor to retrieve.
        // Returns: The requested Vendor entity or a 404 Not Found response if not found.
        [HttpGet("{id}")]
        public async Task<ActionResult<Vendor>> GetVendor(string id)
        {
            var vendor = await _vendorService.GetVendorByIdAsync(id);
            if (vendor == null)
                return NotFound();
            return Ok(vendor);
        }

        // Method: CreateVendor
        // Purpose: Creates a new vendor in the database.
        // Parameters: vendorDto - The data transfer object containing vendor information.
        // Returns: The created Vendor entity or a Bad Request response if the email is already in use.
        [HttpPost]
        //[Authorize(Roles = "Administrator")] // Commented out for testing
        public async Task<ActionResult<Vendor>> CreateVendor([FromBody] CreateVendorDTO vendorDto)
        {

            // Check if email is already in use
            if (await _vendorService.IsEmailInUseAsync(vendorDto.Email))
            {
                return BadRequest(new { message = "Email is already in use." });
            }

            var vendor = new Vendor
            {
                Name = vendorDto.Name,
                Email = vendorDto.Email,
                PhoneNumber = vendorDto.PhoneNumber,
                Description = vendorDto.Description,
                Password = vendorDto.Password,
                UserType = "Vendor"
            };

            var createdVendor = await _vendorService.CreateVendorAsync(vendor);
            return CreatedAtAction(nameof(GetVendor), new { id = createdVendor.Id }, createdVendor);
        }

        // Method: AddRating
        // Purpose: Adds a rating for a vendor from a customer.
        // Parameters: vendorId - The ID of the vendor to rate; ratingDto - The rating information.
        // Returns: A 200 OK response if successful; otherwise, a 404 Not Found response.
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

        // Method: GetVendorRatings
        // Purpose: Retrieves all ratings for a specific vendor.
        // Parameters: vendorId - The ID of the vendor to get ratings for.
        // Returns: A list of VendorRating entities associated with the vendor
        [HttpGet("{vendorId}/ratings")]
        public async Task<ActionResult<List<VendorRating>>> GetVendorRatings(string vendorId)
        {
            var ratings = await _vendorService.GetVendorRatingsAsync(vendorId);
            return Ok(ratings);
        }

        // Method: LoginVendor
        // Purpose: Authenticates a vendor based on email and password.
        // Parameters: vendorDto - The data transfer object containing login information.
        // Returns: The authenticated Vendor entity or an error message if authentication fails.
        [HttpPost("login")]
        public async Task<ActionResult<Vendor>> LoginVendor([FromBody] LoginDTO vendorDto)
        {

            // Check if the vendor with the provided email exists
            var vendor = await _vendorService.GetVendorByEmail(vendorDto.Email);
            if (vendor == null)
            {
                return BadRequest(new { message = "No user found with this email." });
            }

            // Verify the password (assuming you have a method for this)
            if (!VerifyPassword(vendorDto.Password, vendor.Password)) // Use your hashing/verification method
            {
                return BadRequest(new { message = "Incorrect password." });
            }

            // If both checks pass, return the vendor data (or a token, etc.)
            return Ok(vendor);
        }

        // Method: VerifyPassword
        // Purpose: Verifies the password for a vendor.
        // Parameters: inputPassword - The password provided by the user; storedPassword - The hashed password stored in the database.
        // Returns: True if the passwords match; otherwise, false.
        private bool VerifyPassword(string inputPassword, string storedPassword)
        {
            // Implement your password verification logic here
            // For example, if you are hashing passwords, use the same hash function to hash inputPassword
            // and compare it with storedPassword.

            return inputPassword == storedPassword; // Replace this with your actual password verification logic
        }




    }
}