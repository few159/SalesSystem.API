using AutoMapper;
using SalesSystem.Application.DTOs.Sale;
using SalesSystem.Domain.Entities;

namespace SalesSystem.Application.Mappings;

public class SaleProfile : Profile
{
    public SaleProfile()
    {
        CreateMap<Sale, SaleDto>()
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.CustomerName))
            .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.BranchName))
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
            .ForMember(dest => dest.IsCancelled, opt => opt.MapFrom(src => src.IsCancelled));

        CreateMap<SaleItem, SaleItemDto>()
            .ForMember(dest => dest.ProductTitle, opt => opt.MapFrom(src => src.ProductTitle))
            .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.Total));

        // Request → Domain (para criação)
        CreateMap<CreateSaleRequest, Sale>();
        CreateMap<CreateSaleItemRequest, SaleItem>();
    }
}
