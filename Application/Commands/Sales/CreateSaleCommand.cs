using Application.DTOs;
using AutoMapper;
using Domain.Enitities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Commands.Sales;

public class CreateSaleCommand : IRequest<SaleDto>
{
    public string CustomerId { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string BranchId { get; set; } = string.Empty;
    public string BranchName { get; set; } = string.Empty;
    public List<CreateSaleItemRequest> Items { get; set; } = [];
}

public class CreateSaleCommandHandler(
    IMapper _mapper,
    ISaleRepository _saleRepository
) : IRequestHandler<CreateSaleCommand, SaleDto>
{
    public async Task<SaleDto> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
    {
        var sale = new Sale(
            customerId: request.CustomerId,
            customerName: request.CustomerName,
            branchId: request.BranchId,
            branchName: request.BranchName
        );

        foreach (var item in request.Items)
        {
            // Aqui você pode buscar dados do produto (ex: título, preço) no domínio real.
            // Para o exemplo, simula valores fixos:
            var productTitle = "Produto Simulado"; // você substituirá por um fetch real se necessário
            var unitPrice = 100.00m;

            sale.AddItem(item.ProductId, productTitle, unitPrice, item.Quantity);
        }

        await _saleRepository.InsertAsync(sale);

        // Aqui você pode publicar um domínio ou logar um evento como SaleCreated
        // e.g.: _eventBus.Publish(new SaleCreatedEvent(sale));

        return _mapper.Map<SaleDto>(sale);
    }
}