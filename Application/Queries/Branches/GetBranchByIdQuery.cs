using AutoMapper;
using MediatR;
using SalesSystem.Application.DTOs.Branch;
using SalesSystem.Application.Interfaces.Repositories.Stores;

namespace SalesSystem.Application.Queries.Branches;

public sealed record GetBranchQuery(string BranchId) : IRequest<BranchDto>;

internal class GetBranchQueryHandler(IBranchSnapshotStore _branchSnapshotStore, IMapper _mapper)
    : IRequestHandler<GetBranchQuery, BranchDto>
{
    public async Task<BranchDto> Handle(GetBranchQuery request, CancellationToken cancellationToken)
    {
        var product = await _branchSnapshotStore.GetAsync(request.BranchId) ??
                      throw new Exception("Produto não encontrado");

        return _mapper.Map<BranchDto>(product);
    }
}