using Application.Commands.Sales;
using Application.DTOs;
using Application.Queries.Sales;
using Microsoft.AspNetCore.Mvc;
using SalesSystem.API.Controllers.Base;

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
    public async Task<IActionResult> GetSales()
    {
        var result = await Mediator.Send(new GetSalesQuery());
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateSale([FromBody] CreateSaleCommand command)
    {
        var result = await Mediator.Send(command);
        return CreatedAtAction(nameof(CreateSale), new { id = result.Id }, result);
    }
    
    [HttpPost("{saleId}/cancel-item")]
    public async Task<IActionResult> CancelSaleItem([FromRoute] Guid saleId)
    {
        throw new NotImplementedException();
        // var result = await Mediator.Send(command);
        // return CreatedAtAction(nameof(CreateSale), new { id = result.Id }, result);
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateSale([FromBody] CreateSaleCommand command)
    {
        throw new NotImplementedException();
        var result = await Mediator.Send(command);
        return CreatedAtAction(nameof(CreateSale), new { id = result.Id }, result);
    }
    
    [HttpDelete("{saleId}")]
    public async Task<IActionResult> DeleteSale([FromRoute] Guid saleId)
    {
        throw new NotImplementedException();
        // var result = await Mediator.Send(command);
        // return CreatedAtAction(nameof(CreateSale), new { id = result.Id }, result);
    }
}