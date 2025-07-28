using AutoMapper;
using SalesSystem.Application.DTOs.Product;
using SalesSystem.Domain.Enitities.Snapshot;

namespace SalesSystem.Application.Mappings;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<ProductSnapshot, ProductDto>().ReverseMap();
        CreateMap<RatingSnapshot, RatingDto>().ReverseMap();
        CreateMap<CreateProductRequest, ProductSnapshot>();
    }
}