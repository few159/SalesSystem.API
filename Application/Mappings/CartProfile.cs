using AutoMapper;
using SalesSystem.Application.DTOs.Cart;
using SalesSystem.Domain.Entities;

namespace SalesSystem.Application.Mappings;

public class CartProfile : Profile
{
    public CartProfile()
    {
        CreateMap<Cart, CartDto>().ReverseMap();
        CreateMap<CartItem, CartItemDto>().ReverseMap();
        CreateMap<CreateCartRequest, Cart>();
    }
}