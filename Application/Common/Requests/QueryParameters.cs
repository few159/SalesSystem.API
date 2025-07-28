namespace SalesSystem.Application.Common.Requests;

public record QueryParameters
{
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;
    public string? Order { get; set; }

    public Dictionary<string, string>? Filters { get; set; }
}