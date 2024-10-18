// This file contains the Order class which represents an order in the system. It contains the properties of an order and the list of order items in the order.
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace E_commerce_system.Entities
{
    //Order class to represent an order
    public class Order
    {
        //Id of the order
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        //User Id
        [BsonElement("user_id"), BsonRepresentation(BsonType.String)]
        public string? UserId { get; set; }

        //Shipping Address
        [BsonElement("shipping_address"), BsonRepresentation(BsonType.String)]
        public string? ShippingAddress { get; set; }

        //Status of the order
        [BsonElement("status"), BsonRepresentation(BsonType.String)]
        public string? Status { get; set; }

        //Order Date
        [BsonElement("order_date"), BsonRepresentation(BsonType.DateTime)]
        public DateTime OrderDate { get; set; }

        //Order Total
        [BsonElement("order_total"), BsonRepresentation(BsonType.Decimal128)]
        public decimal OrderTotal { get; set; }

        //Order Items
        [BsonElement("order_items")]
        public List<OrderItems>? OrderItems { get; set; }
    }
}
