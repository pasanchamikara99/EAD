using E_commerce_system.Data;
using E_commerce_system.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

/*
* File: CustomerController.cs
* Author: Pasan Chamikara
* Purpose: Handles customer-related operations including retrieving customer by ID, registration, login, and account activation/deactivation.
*/

namespace E_commerce_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {

        private readonly IMongoCollection<Customer>? _customers;


        public CustomerController(MongoDbService mongoDbService) {

            _customers = mongoDbService.Database?.GetCollection<Customer>("customer");
        }


        // Method: GetCustomers
        // Purpose: Retrieves a list of all customers.
        // Returns: A list of Customer objects.
        [HttpGet("getAllcustomers")]
        public async Task<IEnumerable<Customer>> Get() { 
            return await _customers.Find(FilterDefinition<Customer>.Empty).ToListAsync();
        }



        // Method: GetById
        // Purpose: Retrieves a customer by their unique ID.
        // Parameters: id - The unique identifier for the customer.
        // Returns: The customer object if found, otherwise NotFound.
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer?>> GetById(string id) 
        {
            var filter = Builders<Customer>.Filter.Eq(x => x.Id, id);
            var customer = await _customers.Find(filter).FirstOrDefaultAsync();
            return customer is not null ? Ok(customer) : NotFound();
        }

        // Method: Post (Register)
        // Purpose: Registers a new customer. The customer's initial status is set to 'Inactive'.
        // Parameters: customer - The customer object containing registration information.
        // Returns: The newly created customer object.
        [HttpPost("register")]
        public async Task<ActionResult> Post(Customer customer) {
            customer.Status = "InActive";
            await _customers.InsertOneAsync(customer);
            return CreatedAtAction(nameof(GetById), new { id = customer.Id} , customer);
        }

        // Method: Login
        // Purpose: Authenticates a customer based on email and password.
        // Parameters: loginDTO - Contains the email and password for login.
        // Returns: The customer object if authentication is successful, otherwise an appropriate error response.
        [HttpPost("login")]
        public async Task<ActionResult> Login(DTO.LoginDTO loginDTO)
        {
            var existingCustomer = await _customers.Find(c => c.Email == loginDTO.Email).FirstOrDefaultAsync();

            if (existingCustomer == null)
            {
                return NotFound("Customer not found.");
            }

            if (existingCustomer.Password != loginDTO.Password)
            {
                return Unauthorized("Invalid password.");
            }

            if (existingCustomer.Status != "Active")
            {
                return Forbid("Customer account is not active.");
            }

            return Ok(existingCustomer);
        }


        // Method: DeactivateAccount
        // Purpose: Deactivates a customer's account by setting the status to 'Inactive'.
        // Parameters: customerId - The unique identifier of the customer whose account is to be deactivated.
        // Returns: A success message if the account is deactivated, otherwise an appropriate error response.
        [HttpPost("deactivate/{customerId}")]
        public async Task<ActionResult> DeactivateAccount(string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
            {
                return BadRequest("Customer ID is required.");
            }

            // Find the customer by ID
            var existingCustomer = await _customers.Find(c => c.Id == customerId).FirstOrDefaultAsync();

            if (existingCustomer == null)
            {
                return NotFound("Customer not found.");
            }

     
            // Update the status to 'Inactive'
            var update = Builders<Customer>.Update.Set(c => c.Status, "Inactive");
            await _customers.UpdateOneAsync(c => c.Id == customerId, update);

            return Ok("Account has been deactivated.");
        }

        // Method: ActiveAccount
        // Purpose: Activates a customer's account by setting the status to 'Active'.
        // Parameters: customerId - The unique identifier of the customer whose account is to be activated.
        // Returns: A success message if the account is activated, otherwise an appropriate error response.
        [HttpPost("activate/{customerId}")]
        public async Task<ActionResult> ActiveAccount(string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
            {
                return BadRequest("Customer ID is required.");
            }

            // Find the customer by ID
            var existingCustomer = await _customers.Find(c => c.Id == customerId).FirstOrDefaultAsync();

            if (existingCustomer == null)
            {
                return NotFound("Customer not found.");
            }


            // Update the status to 'Active'
            var update = Builders<Customer>.Update.Set(c => c.Status, "Active");
            await _customers.UpdateOneAsync(c => c.Id == customerId, update);

            return Ok("Account has been activated.");
        }

    }
}
