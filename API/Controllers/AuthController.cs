using Microsoft.AspNetCore.Mvc;
using SalesSystem.API.Controllers.Base;
using SalesSystem.Application.Commands.Auth;
using SalesSystem.Application.DTOs.Auth;

namespace SalesSystem.API.Controllers;

public class AuthController : ApiBaseController
{
    [HttpPost("login")]
    [ProducesResponseType(typeof(IEnumerable<TokenDto>), 200)]
    public async Task<IActionResult> Login([FromBody] AuthenticateUserCommand command)
    {
        var result = await Mediator.Send(command);
        return Ok(result);
    }
}