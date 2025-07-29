using Microsoft.AspNetCore.Mvc;
using SalesSystem.API.Controllers.Base;
using SalesSystem.Application.DTOs.Branch;
using SalesSystem.Application.Queries.Branches;

namespace SalesSystem.API.Controllers;

public class BranchController : ApiBaseController
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BranchDto>), 200)]
    public async Task<IActionResult> GetBranches([FromQuery] GetBranchesQuery query)
    {
        query.Filters = GetFiltersFromRequest();

        var result = await Mediator.Send(new GetBranchesQuery());
        return Ok(result);
    }

    [HttpGet("{branchId}")]
    public async Task<IActionResult> GetBranch([FromRoute] Guid branchId)
    {
        var result = await Mediator.Send(new GetBranchQuery(branchId.ToString()));
        return Ok(result);
    }
}