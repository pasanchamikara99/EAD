using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace E_commerce_system.Entities
{
    public class OrderItems
    {
        [BsonElement("product_id"), BsonRepresentation(BsonType.String)]
        public string ProductId { get; set; }


        [BsonElement("product_name"), BsonRepresentation(BsonType.String)]
        public string ProductName { get; set; }


        [BsonElement("quantity"), BsonRepresentation(BsonType.Int32)]
        public int Quantity { get; set; }


        [BsonElement("price"), BsonRepresentation(BsonType.Double)]
        public Double Price { get; set; }

        //Vendor Id
        [BsonElement("vendor_id"), BsonRepresentation(BsonType.String)]
        public string? VendorId { get; set; }

    }
}
