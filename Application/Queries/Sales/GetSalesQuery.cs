using Application.DTOs;
using AutoMapper;
using Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Sales;

public sealed record GetSalesQuery() : IRequest<IEnumerable<SaleDto>>;

internal class GetSalesQueryHandler(ISaleRepository _saleRepository, IMapper _mapper)
    : IRequestHandler<GetSalesQuery, IEnumerable<SaleDto>>
{
    public async Task<IEnumerable<SaleDto>> Handle(GetSalesQuery request, CancellationToken cancellationToken)
    {
        var sales = await _saleRepository.Query().ToListAsync();
        return sales.Select(s => _mapper.Map<SaleDto>(s));
    }
}