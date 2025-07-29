using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SalesSystem.Domain.Enitities.Snapshot;

public class CategorySnapshot
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public List<CategoryDataSnapshot> Data { get; set; } = [];
    public int TotalItems { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
}

public class CategoryDataSnapshot
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public RatingSnapshot Rating { get; set; } = new();
}
