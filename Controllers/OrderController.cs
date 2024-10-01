using E_commerce_system.Data;
using E_commerce_system.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace E_commerce_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMongoCollection<Order>? _orders;

        public OrderController(MongoDbService mongoDbService)
        {
            _orders = mongoDbService.Database?.GetCollection<Order>("order");
        }

        //Get all Orders
        [HttpGet]
        public async Task<IEnumerable<Order>> Get()
        {
            return await _orders.Find(FilterDefinition<Order>.Empty).ToListAsync();
        }

        //Create Order
        [HttpPost]
        public async Task<ActionResult> Post(Order order)
        {
            await _orders.InsertOneAsync(order);
            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }

        //Get Order by Id
        [HttpGet("{id}")]
        public async Task<ActionResult<Order?>> GetById(string id)
        {
            var filter = Builders<Order>.Filter.Eq(x => x.Id, id);
            var order = await _orders.Find(filter).FirstOrDefaultAsync();
            return order is not null ? Ok(order) : NotFound();
        }

        //Update Order if not delivered or dispatched yet
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, Order order)
        {
            var filter = Builders<Order>.Filter.Eq(x => x.Id, id);
            if (order is null)
            {
                return NotFound();
            }
            if (order.Status == "Delivered" || order.Status == "Dispatched")
            {
                return BadRequest("Cannot update order");
            }
            var update = Builders<Order>.Update
                .Set(x => x.UserId, order.UserId)
                .Set(x => x.ShippingAddress, order.ShippingAddress)
                .Set(x => x.Status, order.Status)
                .Set(x => x.OrderDate, order.OrderDate)
                //.Set(x => x.OrderItems, order.OrderItems)
                .Set(x => x.OrderTotal, order.OrderTotal);
            var result = await _orders.UpdateOneAsync(filter, update);
            return result.IsAcknowledged ? Ok(order) : NotFound();
        }

        //Detele Order
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var filter = Builders<Order>.Filter.Eq(x => x.Id, id);
            var result = await _orders.DeleteOneAsync(filter);
            return result.IsAcknowledged ? Ok() : NotFound();
        }

        //Cancel order if not delivered or dispatched yet
        [HttpPut("{id}/cancel")]
        public async Task<ActionResult> CancelOrder(string id)
        {
            var filter = Builders<Order>.Filter.Eq(x => x.Id, id);
            var order = await _orders.Find(filter).FirstOrDefaultAsync();
            if (order is null)
            {
                return NotFound();
            }
            if (order.Status == "Delivered" || order.Status == "Dispatched")
            {
                return BadRequest("Cannot cancel order");
            }
            var update = Builders<Order>.Update.Set(x => x.Status, "Cancelled");
            var result = await _orders.UpdateOneAsync(filter, update);
            return result.IsAcknowledged ? Ok() : NotFound();

        }

    }
}
