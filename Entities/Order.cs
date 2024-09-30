using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace E_commerce_system.Entities
{
    public class Order
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }


        [BsonElement("user_id"), BsonRepresentation(BsonType.String)]
        public string? UserId { get; set; }


        [BsonElement("shipping_address"), BsonRepresentation(BsonType.String)]
        public string? ShippingAddress { get; set; }


        [BsonElement("status"), BsonRepresentation(BsonType.String)]
        public string? Status { get; set; }


        [BsonElement("order_date"), BsonRepresentation(BsonType.DateTime)]
        public DateTime OrderDate { get; set; }


        [BsonElement("order_total"), BsonRepresentation(BsonType.Decimal128)]
        public decimal OrderTotal { get; set; }

        //Order Items as an array
        //[BsonElement("order_items"), BsonRepresentation(BsonType.String)]
        //public List<string>? OrderItems { get; set; }

    }
}
