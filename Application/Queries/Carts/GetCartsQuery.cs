using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SalesSystem.Application.Common.Requests;
using SalesSystem.Application.DTOs.Cart;
using SalesSystem.Application.Interfaces.Repositories;
using SalesSystem.Shared.Extensions;

namespace SalesSystem.Application.Queries.Carts;

public sealed record GetCartsQuery() : QueryParameters, IRequest<IEnumerable<CartDto>>;

internal class GetCartsQueryHandler(ICartRepository _cartRepository, IMapper _mapper)
    : IRequestHandler<GetCartsQuery, IEnumerable<CartDto>>
{
    public async Task<IEnumerable<CartDto>> Handle(GetCartsQuery request, CancellationToken cancellationToken)
    {
        var query = _cartRepository.Query(request.Filters, request.Order);

        var carts = await query
            .Paginate(request.Page, request.Size)
            .ToListAsync();

        return carts.Select(s => _mapper.Map<CartDto>(s));
    }
}