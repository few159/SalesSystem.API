using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SalesSystem.Domain.Enitities.Snapshot;

public class ProductSnapshot
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;

    public RatingSnapshot Rating { get; set; } = new();
}

public class RatingSnapshot
{
    public double Rate { get; set; }
    public int Count { get; set; }
}