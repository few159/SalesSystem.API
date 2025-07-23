using Application.DTOs;
using AutoMapper;
using Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Sales;

public sealed record GetSaleQuery(Guid CustomerId) : IRequest<SaleDto>;

internal class GetSaleQueryHandler(ISaleRepository _saleRepository, IMapper _mapper)
    : IRequestHandler<GetSaleQuery, SaleDto>
{
    public async Task<SaleDto> Handle(GetSaleQuery request, CancellationToken cancellationToken)
    {
        
        var sale = await _saleRepository.Query().FirstAsync(x => x.Id == request.CustomerId);
        return _mapper.Map<SaleDto>(sale);
    }
}