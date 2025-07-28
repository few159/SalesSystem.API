using MediatR;
using Microsoft.AspNetCore.Mvc;
using SalesSystem.Shared.Extensions;

namespace SalesSystem.API.Controllers.Base;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiBaseController: ControllerBase
{
    private ISender? _mediator;
    
    protected ISender Mediator =>
        _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    protected Dictionary<string, string> GetFiltersFromRequest()
    {
        return Request.Query.GetFilters();
    }
}