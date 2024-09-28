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

        public CustomerController(MongoDbService mongoDbService) {

            _customers = mongoDbService.Database?.GetCollection<Customer>("customer");
        }

        [HttpGet]
        public async Task<IEnumerable<Customer>> Get() { 
            return await _customers.Find(FilterDefinition<Customer>.Empty).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Customer?>> GetById(string id) 
        {
            var filter = Builders<Customer>.Filter.Eq(x => x.Id, id);
            var customer = await _customers.Find(filter).FirstOrDefaultAsync();
            return customer is not null ? Ok(customer) : NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> Post(Customer customer) {
            await _customers.InsertOneAsync(customer);
            return CreatedAtAction(nameof(GetById), new { id = customer.Id} , customer);
        }
    }
}
