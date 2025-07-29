using AutoMapper;
using MediatR;
using SalesSystem.Application.DTOs.Cart;
using SalesSystem.Application.Interfaces.Repositories;
using SalesSystem.Application.Interfaces.Repositories.Base;
using SalesSystem.Application.Interfaces.Repositories.Stores;
using SalesSystem.Domain.Entities;

namespace SalesSystem.Application.Commands.Carts;

public class CreateCartCommand : IRequest<CartDto>
{
    public Guid Id { get; set; }
    public int UserId { get; set; } = 0;
    public DateTime Date { get; set; } = DateTime.Now;
    public List<CartItemDto> Products { get; set; } = new();
}

public class CreateCartCommandHandler(
    IMapper mapper,
    ICartRepository cartRepository,
    IProductSnapshotStore productSnapshotStore,
    IUnitOfWork uow
) : IRequestHandler<CreateCartCommand, CartDto>
{
    public async Task<CartDto> Handle(CreateCartCommand request, CancellationToken cancellationToken)
    {
        await uow.BeginTransactionAsync();
        try
        {
            var cart = new Cart();

            foreach (var item in request.Products)
            {
                var product = await productSnapshotStore.GetAsync(item.ProductId)
                              ?? throw new Exception($"Produto {item.ProductId} não encontrado");

                cart.AddProducts(product.Id, item.Quantity);
            }

            await cartRepository.InsertAsync(cart);

            await uow.CommitAsync();

            await uow.SaveAsync();

            return mapper.Map<CartDto>(cart);
        }
        catch (Exception e)
        {
            await uow.RollBackAsync();
            throw;
        }
    }
}