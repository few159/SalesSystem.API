using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SalesSystem.API.Controllers.Base;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiBaseController: ControllerBase
{
    private ISender? _mediator;
    
    protected ISender Mediator =>
        _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}