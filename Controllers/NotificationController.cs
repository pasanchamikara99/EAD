using E_commerce_system.Data.Entities;
using E_commerce_system.Data;
using E_commerce_system.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace E_commerce_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : Controller
    {
        private readonly IMongoCollection<Notification>? _notification;
        public NotificationController(MongoDbService mongoDbService)
        {
            _notification = mongoDbService.Database?.GetCollection<Notification>("notification");     
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Notification?>> GetById(string id)
        {
            var filter = Builders<Notification>.Filter.Eq(x => x.Id, id);
            var notification = await _notification.Find(filter).FirstOrDefaultAsync();
            return notification is not null ? Ok(notification) : NotFound();
        }


        [HttpPost()]
        public async Task<ActionResult> Post(Notification notification)
        {
            Console.WriteLine(notification);
            await _notification.InsertOneAsync(notification);
            return CreatedAtAction(nameof(GetById), new { id = notification.Id }, notification);
        }
    }
}
