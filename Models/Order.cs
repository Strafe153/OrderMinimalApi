using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OrderMinimalApi.Models
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string? CustomerName { get; set; }
        public string? Address { get; set; }
        public string? Product { get; set; }
        public decimal Price { get; set; }
    }
}
