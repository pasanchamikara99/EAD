using E_commerce_system.Data;
using E_commerce_system.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

/*
 * File: OrderController.cs
 * Author: Pasindu Shyminda
 * Purpose: Manages order operations including creating, retrieving, updating, canceling, marking as delivered or dispatched, 
 *          and viewing orders by user or vendor.
 */
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

       
[HttpPost]
public async Task<ActionResult> Post([FromBody] Order order)
{
    if (order == null)
    {
        return BadRequest("Invalid order data");
    }

    // Set the order date to the current time if not provided
    if (order.OrderDate == default)
    {
        order.OrderDate = DateTime.UtcNow;
    }

    // Ensure OrderItems is not null
    if (order.OrderItems == null)
    {
        order.OrderItems = new List<OrderItems>();
    }

    // Log the incoming order for debugging
    Console.WriteLine($"Received order: {System.Text.Json.JsonSerializer.Serialize(order)}");

    try
    {
        // Insert the order into the database
        await _orders.InsertOneAsync(order);

        // Retrieve the inserted order to include all details
        var insertedOrder = await _orders.Find(o => o.Id == order.Id).FirstOrDefaultAsync();
        
        // Log the inserted order to check details
        Console.WriteLine($"Inserted order: {System.Text.Json.JsonSerializer.Serialize(insertedOrder)}");

        return CreatedAtAction(nameof(GetById), new { id = insertedOrder.Id }, insertedOrder);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error inserting order: {ex}");
        return StatusCode(500, "An error occurred while processing your order");
    }
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
        public async Task<ActionResult> Put(string id, Order neworder)
        {
            var filter = Builders<Order>.Filter.Eq(x => x.Id, id);
            var order = await _orders.Find(filter).FirstOrDefaultAsync();
            if (order is null)
            {
                return NotFound();
            }
            if (order.Status == "Delivered")
            {
                return BadRequest("Order is delivered, cannot update order");
            }
            if (order.Status == "Dispatched")
            {
                return BadRequest("Order is dispatched, cannot update order");
            }
            var update = Builders<Order>.Update
                .Set(x => x.UserId, neworder.UserId)
                .Set(x => x.ShippingAddress, neworder.ShippingAddress)
                .Set(x => x.Status, neworder.Status)
                .Set(x => x.OrderDate, neworder.OrderDate)
                .Set(x => x.OrderItems, neworder.OrderItems)
                .Set(x => x.OrderTotal, neworder.OrderTotal);
            var result = await _orders.UpdateOneAsync(filter, update);
            return result.IsAcknowledged ? Ok(neworder) : NotFound();
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
            if (order.Status == "Delivered")
            {
                return BadRequest("Order is delivered, cannot cancel order");
            }
            if (order.Status == "Dispatched")
            {
                return BadRequest("Order is dispatched, cannot cancel order");
            }
            var update = Builders<Order>.Update.Set(x => x.Status, "Cancelled");
            var result = await _orders.UpdateOneAsync(filter, update);
            return result.IsAcknowledged ? Ok() : NotFound();

        }

        //Mark order as delivered
        [HttpPut("{id}/delivered")]
        public async Task<ActionResult> MarkAsDelivered(string id)
        {
            var filter = Builders<Order>.Filter.Eq(x => x.Id, id);
            var order = await _orders.Find(filter).FirstOrDefaultAsync();
            if (order is null)
            {
                return NotFound();
            }
            if (order.Status == "Cancelled")
            {
                return BadRequest("Order is cancelled cannot mark as delivered");
            }
            var update = Builders<Order>.Update.Set(x => x.Status, "Delivered");
            var result = await _orders.UpdateOneAsync(filter, update);
            return result.IsAcknowledged ? Ok() : NotFound();
        }

        //Mark order as dispatched
        [HttpPut("{id}/dispatched")]
        public async Task<ActionResult> MarkAsDispatched(string id)
        {
            var filter = Builders<Order>.Filter.Eq(x => x.Id, id);
            var order = await _orders.Find(filter).FirstOrDefaultAsync();
            if (order is null)
            {
                return NotFound();
            }
            if (order.Status == "Cancelled")
            {
                return BadRequest("Order is cancelled cannot mark as dispatched");
            }
            if (order.Status == "Delivered")
            {
                return BadRequest("Order is delivered cannot mark as dispatched");
            }
            var update = Builders<Order>.Update.Set(x => x.Status, "Dispatched");
            var result = await _orders.UpdateOneAsync(filter, update);
            return result.IsAcknowledged ? Ok() : NotFound();
        }

        //View only specific user orders
        [HttpGet("user/{userId}")]
        public async Task<IEnumerable<Order>> GetUserOrders(string userId)
        {
            var filter = Builders<Order>.Filter.Eq(x => x.UserId, userId);
            var order = await _orders.Find(filter).FirstOrDefaultAsync();

            if (order is null)
            {
                return null;
            }
            return await _orders.Find(filter).ToListAsync();
        }

        //View only specific vender orders
        //[HttpGet("vendor/{vendorId}")]
        //public async Task<IEnumerable<Order>> GetVendorOrders(string vendorId)
        //{
        //    var filter = Builders<Order>.Filter.Eq(x => x.OrderItems.Any(x => x.VendorId == vendorId));
        //    return await _orders.Find(filter).ToListAsync();
        //}

    }
}
