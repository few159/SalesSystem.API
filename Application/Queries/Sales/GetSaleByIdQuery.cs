using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SalesSystem.Application.DTOs.Sale;
using SalesSystem.Application.Interfaces.Repositories;

namespace SalesSystem.Application.Queries.Sales;

public sealed record GetSaleQuery(Guid SaleId) : IRequest<SaleDto>;

internal class GetSaleQueryHandler(ISaleRepository _saleRepository, IMapper _mapper)
    : IRequestHandler<GetSaleQuery, SaleDto>
{
    public async Task<SaleDto> Handle(GetSaleQuery request, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.Query()
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == request.SaleId) ?? throw new Exception("Venda não encontrada");
        
        return _mapper.Map<SaleDto>(sale);
    }
}