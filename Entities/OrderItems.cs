using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace E_commerce_system.Entities
{
    public class OrderItems
    {
        [BsonElement("product_id"), BsonRepresentation(BsonType.String)]
        public string ProductId { get; set; }


        [BsonElement("quantity"), BsonRepresentation(BsonType.Int32)]
        public int Quantity { get; set; }


        [BsonElement("price"), BsonRepresentation(BsonType.Double)]
        public Double Price { get; set; }

    }
}
