using E_commerce_system.Data;
using E_commerce_system.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace E_commerce_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {

        private readonly IMongoCollection<Customer>? _customers;

        public CustomerController(MongoDbService mongoDbService)
        {

            _customers = mongoDbService.Database?.GetCollection<Customer>("customer");
        }

        [HttpGet("getAllcustomers")]
        public async Task<IEnumerable<Customer>> Get()
        {
            return await _customers.Find(FilterDefinition<Customer>.Empty).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Customer?>> GetById(string id)
        {
            var filter = Builders<Customer>.Filter.Eq(x => x.Id, id);
            var customer = await _customers.Find(filter).FirstOrDefaultAsync();
            return customer is not null ? Ok(customer) : NotFound();
        }

        [HttpPost("/register")]
        public async Task<ActionResult> Post(Customer customer)
        {
            customer.Status = "InActive";
            await _customers.InsertOneAsync(customer);
            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
        }

        [HttpPost("/login")]
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


        [HttpPost("/deactivate/{customerId}")]
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

    }
}