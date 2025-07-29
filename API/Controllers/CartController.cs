using Microsoft.AspNetCore.Mvc;
using SalesSystem.API.Controllers.Base;
using SalesSystem.Application.Commands.Carts;
using SalesSystem.Application.DTOs.Cart;
using SalesSystem.Application.Queries.Carts;

namespace SalesSystem.API.Controllers;

public class CartController : ApiBaseController
{
    [HttpGet("{cartId}")]
    public async Task<IActionResult> GetCart([FromRoute] Guid cartId)
    {
        var result = await Mediator.Send(new GetCartQuery(cartId));
        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CartDto>), 200)]
    public async Task<IActionResult> GetCarts([FromQuery] GetCartsQuery query)
    {
        query.Filters = GetFiltersFromRequest();

        var result = await Mediator.Send(new GetCartsQuery());
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCart([FromBody] CreateCartCommand command)
    {
        var result = await Mediator.Send(command);
        return CreatedAtAction(nameof(CreateCart), new { id = result.Id }, result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCart([FromBody] CreateCartCommand command)
    {
        var result = await Mediator.Send(command);
        return CreatedAtAction(nameof(CreateCart), new { id = result.Id }, result);
    }


    [HttpDelete("{cartId}/cancel")]
    public async Task<IActionResult> CancelCart([FromRoute] Guid cartId)
    {
        var result = await Mediator.Send(new CancelCartCommand() { CartId = cartId });
        return Ok(result);
    }
}