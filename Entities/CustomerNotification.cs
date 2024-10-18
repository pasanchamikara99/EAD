using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace E_commerce_system.Entities
{
    public class CustomerNotification
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("customer_id"), BsonRepresentation(BsonType.String)]
        public string CustomerId { get; set; }

        [BsonElement("order_id"), BsonRepresentation(BsonType.String)]
        public string OrderId { get; set; }

        [BsonElement("message"), BsonRepresentation(BsonType.String)]
        public string Message { get; set; }

        //current timestamp
        //[BsonElement("timestamp"), BsonRepresentation(BsonType.DateTime)]
        //public long Timestamp { get; set; }
    }
}
