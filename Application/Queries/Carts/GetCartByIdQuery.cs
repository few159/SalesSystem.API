using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SalesSystem.Application.DTOs.Cart;
using SalesSystem.Application.Interfaces.Repositories;

namespace SalesSystem.Application.Queries.Carts;

public sealed record GetCartQuery(Guid CartId) : IRequest<CartDto>;

internal class GetCartQueryHandler(ICartRepository _cartRepository, IMapper _mapper)
    : IRequestHandler<GetCartQuery, CartDto>
{
    public async Task<CartDto> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.Query()
            .FirstOrDefaultAsync(x => x.Id == request.CartId) ?? throw new Exception("Carrinho não encontrado");
        
        return _mapper.Map<CartDto>(cart);
    }
}