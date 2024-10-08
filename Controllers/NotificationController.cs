using E_commerce_system.Data.Entities;
using E_commerce_system.Data;
using E_commerce_system.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

/*
 * File: NotificationController.cs
 * Author: Pasan Chamikara
 * Purpose: Manages notification operations, including retrieving notifications by ID and vendor ID, and creating new notifications.
 */
namespace E_commerce_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : Controller
    {
        private readonly IMongoCollection<Notification>? _notification; // Collection of notifications

        // Constructor: Initializes the controller with the MongoDB collection for notifications.
        public NotificationController(MongoDbService mongoDbService)
        {
            _notification = mongoDbService.Database?.GetCollection<Notification>("notification");     
        }


        // Method: GetById
        // Purpose: Retrieves a notification by its ID.
        // Parameter: id - The ID of the notification to retrieve.
        // Returns: The notification if found, otherwise NotFound result.
        [HttpGet("{id}")]
        public async Task<ActionResult<Notification?>> GetById(string id)
        {
            var filter = Builders<Notification>.Filter.Eq(x => x.Id, id);
            var notification = await _notification.Find(filter).FirstOrDefaultAsync();
            return notification is not null ? Ok(notification) : NotFound();
        }


        // Method: Post
        // Purpose: Creates a new notification.
        // Parameter: notification - The notification to create.
        // Returns: A Created result with the location of the new notification.
        [HttpPost()]
        public async Task<ActionResult> Post(Notification notification)
        {
            Console.WriteLine(notification);
            await _notification.InsertOneAsync(notification);
            return CreatedAtAction(nameof(GetById), new { id = notification.Id }, notification);
        }

        // Method: GetByVendorId
        // Purpose: Retrieves a notification by vendor ID.
        // Parameter: id - The ID of the vendor whose notifications are to be retrieved.
        // Returns: The notification if found, otherwise NotFound result.
        [HttpGet("getbyvendor/{id}")]
        public async Task<ActionResult<Notification?>> GetByVendorId(string id)
        {
            // Create a filter to find notifications by vendor ID
            var filter = Builders<Notification>.Filter.Eq(x => x.VendorId, id);
            var notification = await _notification.Find(filter).FirstOrDefaultAsync();
            return notification is not null ? Ok(notification) : NotFound();
        }
    }
}
