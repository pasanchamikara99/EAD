using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace E_commerce_system.Entities
{
    public class Product
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("product_name"), BsonRepresentation(BsonType.String)]
        public string ProductName { get; set; }

        [BsonElement("product_description"), BsonRepresentation(BsonType.String)]
        public string ProductDescription { get; set; }

        [BsonElement("product_category"), BsonRepresentation(BsonType.String)]
        public string ProductCategory { get; set; }

        [BsonElement("product_quantity"), BsonRepresentation(BsonType.String)]
        public string ProductQuantity { get; set; }

        [BsonElement("unit_price"), BsonRepresentation(BsonType.String)]
        public string UnitPrice { get; set; }

        [BsonElement("vendor_id"), BsonRepresentation(BsonType.String)]
        public string VendorId { get; set; }

        [BsonElement("vendor_name"), BsonRepresentation(BsonType.String)]
        public string? VendorName { get; set; }

        [BsonElement("status"), BsonRepresentation(BsonType.String)]
        public string Status { get; set; } = "Pending";

        [BsonElement("image_url"), BsonRepresentation(BsonType.String)]
        public string? Image { get; set; }
    }
}
