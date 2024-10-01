using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace E_commerce_system.Entities
{
    public class Customer
    {
        [BsonId]
        [BsonElement("_id") , BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("customer_name"), BsonRepresentation(BsonType.String)]
        [Required(ErrorMessage = "Customer name is required.")]
        [StringLength(100, ErrorMessage = "Customer name can't be longer than 100 characters.")]
        public string? CustomerName { get; set; }

        [BsonElement("email"), BsonRepresentation(BsonType.String)]
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string? Email { get; set; }

        [BsonElement("account_status"), BsonRepresentation(BsonType.String)]
        public string? Status { get; set; }

        [BsonElement("password"), BsonRepresentation(BsonType.String)]
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 50 characters.")]
        public string? Password { get; set; }
    }
}
