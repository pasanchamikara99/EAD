using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace E_commerce_system.Entities
{
    public class Customer
    {
        [BsonId]
        [BsonElement("_id") , BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("customer_name"), BsonRepresentation(BsonType.String)]
        public string? CustomerName { get; set; }

        [BsonElement("email"), BsonRepresentation(BsonType.String)]
        public string? Email { get; set; }
    }
}
