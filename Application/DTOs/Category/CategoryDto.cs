using SalesSystem.Application.DTOs.Product;

namespace SalesSystem.Application.DTOs.Category;

public class CategoryDto
{
    public List<CategoryDataDto> Data { get; set; } = [];
    public int TotalItems { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
}

public class CategoryDataDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public RatingDto Rating { get; set; } = new();
}
