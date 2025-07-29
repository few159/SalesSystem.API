using Microsoft.AspNetCore.Mvc;
using SalesSystem.API.Controllers.Base;
using SalesSystem.Application.DTOs.Product;
using SalesSystem.Application.Queries.Products;

namespace SalesSystem.API.Controllers;

public class ProductController : ApiBaseController
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProductDto>), 200)]
    public async Task<IActionResult> GetProducts([FromQuery] GetProductsQuery query)
    {
        query.Filters = GetFiltersFromRequest();

        var result = await Mediator.Send(new GetProductsQuery());
        return Ok(result);
    }

    [HttpGet("product/{productId}")]
    public async Task<IActionResult> GetProduct([FromRoute] Guid productId)
    {
        var result = await Mediator.Send(new GetProductQuery(productId));
        return Ok(result);
    }
}