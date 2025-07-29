using AutoMapper;
using MediatR;
using SalesSystem.Application.Common.Requests;
using SalesSystem.Application.DTOs.Branch;
using SalesSystem.Application.Interfaces.Repositories.Stores;

namespace SalesSystem.Application.Queries.Branches;

public sealed record GetBranchesQuery() : QueryParameters, IRequest<IEnumerable<BranchDto>>;

internal class GetBranchesQueryHandler(IBranchSnapshotStore _branchSnapshotStore, IMapper _mapper)
    : IRequestHandler<GetBranchesQuery, IEnumerable<BranchDto>>
{
    public async Task<IEnumerable<BranchDto>> Handle(GetBranchesQuery request, CancellationToken cancellationToken)
    {
        var branches = await _branchSnapshotStore.GetAllAsync();

        return branches.Select(s => _mapper.Map<BranchDto>(s));
    }
}