using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace E_commerce_system.Entities
{ 
    public class VendorRatings
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        
        [BsonElement("vendor_id"), BsonRepresentation(BsonType.String)]
        public string VendorId { get; set; }

        
        [BsonElement("customer_id"), BsonRepresentation(BsonType.String)]
        public string CustomerId { get; set; }

        
        [BsonElement("product_id"), BsonRepresentation(BsonType.String)]
        public string ProductId { get; set; }

        
        [BsonElement("rating"), BsonRepresentation(BsonType.String)]
        public float Rating { get; set; }

        
        [BsonElement("review"), BsonRepresentation(BsonType.String)]
        public string Review { get; set; }
    }
}
