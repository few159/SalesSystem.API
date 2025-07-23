namespace Application.Common.Requests;

public class QueryParameters
{
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;
    public string? Order { get; set; }

    public IDictionary<string, string>? Filters { get; set; }
}