using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace E_commerce_system.Entities
{
    public class Notification
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("vendor_id"), BsonRepresentation(BsonType.String)]
        public string VendorId { get; set; }

        [BsonElement("product_id"), BsonRepresentation(BsonType.String)]
        public string ProductId { get; set; }

        [BsonElement("message"), BsonRepresentation(BsonType.String)]
        public string Message { get; set; }
    }
}
