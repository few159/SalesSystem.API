using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SalesSystem.Application.Common.Requests;
using SalesSystem.Application.DTOs.Sale;
using SalesSystem.Application.Interfaces.Repositories;
using SalesSystem.Shared.Extensions;

namespace SalesSystem.Application.Queries.Sales;

public sealed record GetSalesQuery() : QueryParameters, IRequest<IEnumerable<SaleDto>>;

internal class GetSalesQueryHandler(ISaleRepository _saleRepository, IMapper _mapper)
    : IRequestHandler<GetSalesQuery, IEnumerable<SaleDto>>
{
    public async Task<IEnumerable<SaleDto>> Handle(GetSalesQuery request, CancellationToken cancellationToken)
    {
        var query = _saleRepository.Query(request.Filters, request.Order);

        var sales = await query
            .Include(x => x.Items)
            .Paginate(request.Page, request.Size)
            .ToListAsync();

        return sales.Select(s => _mapper.Map<SaleDto>(s));
    }
}