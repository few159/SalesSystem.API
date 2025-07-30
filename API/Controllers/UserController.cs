using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesSystem.API.Controllers.Base;
using SalesSystem.Application.Commands.User;
using SalesSystem.Application.DTOs.User;

namespace SalesSystem.API.Controllers;

public class UserController : ApiBaseController
{
    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(typeof(IEnumerable<UserDto>), 200)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
    {
        var result = await Mediator.Send(command);
        return CreatedAtAction(nameof(CreateUser), new { id = result.Id }, result);
    }
}