using Microsoft.AspNetCore.Mvc;
using E_commerce_system.Entities;
using E_commerce_system.Data;
using MongoDB.Driver;

namespace E_commerce_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerNotificationController : ControllerBase
    {
        private readonly IMongoCollection<CustomerNotification>? _customerNotifications;

        //Constructor
        public CustomerNotificationController(MongoDbService mongoDbService)
        {
            _customerNotifications = mongoDbService.Database?.GetCollection<CustomerNotification>("customer_notification");
        }

        //Get all Customer Notifications
        [HttpGet]
        public async Task<IEnumerable<CustomerNotification>> Get()
        {
            return await _customerNotifications.Find(FilterDefinition<CustomerNotification>.Empty).ToListAsync();
        }

        //Create Customer Notification
        [HttpPost]
        public async Task<ActionResult> Post(CustomerNotification customerNotification)
        {
            //print customer notification details
            Console.WriteLine(customerNotification);

            await _customerNotifications.InsertOneAsync(customerNotification);
            return CreatedAtAction(nameof(GetById), new { id = customerNotification.Id }, customerNotification);
        }

        //Get Customer Notification by Id
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerNotification?>> GetById(string id)
        {
            var filter = Builders<CustomerNotification>.Filter.Eq(x => x.Id, id);
            var customerNotification = await _customerNotifications.Find(filter).FirstOrDefaultAsync();
            return customerNotification is not null ? Ok(customerNotification) : NotFound();
        }

        //Update Customer Notification
        //[HttpPut("{id}")]
        //public async Task<ActionResult> Put(string id, CustomerNotification newCustomerNotification)
        //{
            //var filter = Builders<CustomerNotification>.Filter.Eq(x => x.Id, id);
            //var customerNotification = await _customerNotifications.Find(filter).FirstOrDefaultAsync();
            //if (customerNotification is null)
            //{
                //return NotFound();
            //}
            //await _customerNotifications.ReplaceOneAsync(filter, newCustomerNotification);
            //return Ok(newCustomerNotification);
        //}


        //Delete Customer Notification
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var filter = Builders<CustomerNotification>.Filter.Eq(x => x.Id, id);
            var customerNotification = await _customerNotifications.Find(filter).FirstOrDefaultAsync();
            if (customerNotification is null)
            {
                return NotFound();
            }
            await _customerNotifications.DeleteOneAsync(filter);
            return Ok();
        }       
    }
}
