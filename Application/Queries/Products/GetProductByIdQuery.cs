using AutoMapper;
using MediatR;
using SalesSystem.Application.DTOs.Product;
using SalesSystem.Application.Interfaces.Repositories.Stores;

namespace SalesSystem.Application.Queries.Products;

public sealed record GetProductQuery(Guid ProductId) : IRequest<ProductDto>;

internal class GetProductQueryHandler(IProductSnapshotStore _productSnapshotStore, IMapper _mapper)
    : IRequestHandler<GetProductQuery, ProductDto>
{
    public async Task<ProductDto> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var product = await _productSnapshotStore.GetAsync(request.ProductId) ??
                      throw new Exception("Produto não encontrado");

        return _mapper.Map<ProductDto>(product);
    }
}