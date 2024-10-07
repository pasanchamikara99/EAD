using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace E_commerce_system.Entities
{
    public class Category
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }


        [BsonElement("category_name"), BsonRepresentation(BsonType.String)]
        public string? CategoryName { get; set; }

    }
}
