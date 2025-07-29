using Microsoft.AspNetCore.Mvc;
using SalesSystem.API.Controllers.Base;
using SalesSystem.Application.Commands.Sale;
using SalesSystem.Application.DTOs.Sale;
using SalesSystem.Application.Queries.Sales;

namespace SalesSystem.API.Controllers;

public class SalesController : ApiBaseController
{
    [HttpGet("{saleId}")]
    public async Task<IActionResult> GetSale([FromRoute] Guid saleId)
    {
        var result = await Mediator.Send(new GetSaleQuery(saleId));
        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<SaleDto>), 200)]
    public async Task<IActionResult> GetSales([FromQuery] GetSalesQuery query)
    {
        query.Filters = GetFiltersFromRequest();

        var result = await Mediator.Send(new GetSalesQuery());
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateSale([FromBody] CreateSaleCommand command)
    {
        var result = await Mediator.Send(command);
        return CreatedAtAction(nameof(CreateSale), new { id = result.Id }, result);
    }

    [HttpPut("{saleId}/cancel")]
    public async Task<IActionResult> CancelSale([FromRoute] Guid saleId)
    {
        var result = await Mediator.Send(new CancelSaleCommand() { SaleId = saleId });
        return Ok(result);
    }
}