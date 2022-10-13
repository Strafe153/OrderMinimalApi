using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OrderMinimalApi.Models;

public class Order
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string CustomerName { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string Product { get; set; } = default!;
    public decimal Price { get; set; }
}
