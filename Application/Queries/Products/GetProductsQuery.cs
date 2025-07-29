using AutoMapper;
using MediatR;
using SalesSystem.Application.Common.Requests;
using SalesSystem.Application.DTOs.Product;
using SalesSystem.Application.Interfaces.Repositories.Stores;

namespace SalesSystem.Application.Queries.Products;

public sealed record GetProductsQuery() : QueryParameters, IRequest<IEnumerable<ProductDto>>;

internal class GetProductsQueryHandler(IProductSnapshotStore _productSnapshotStore, IMapper _mapper)
    : IRequestHandler<GetProductsQuery, IEnumerable<ProductDto>>
{
    public async Task<IEnumerable<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _productSnapshotStore.GetAllAsync();

        return products.Select(p => _mapper.Map<ProductDto>(p));
    }
}