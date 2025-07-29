using AutoMapper;
using SalesSystem.Application.DTOs.Branch;
using SalesSystem.Domain.Entities.Snapshot;

namespace SalesSystem.Application.Mappings;

public class BranchProfile : Profile
{
    public BranchProfile()
    {
        CreateMap<BranchSnapshot, BranchDto>().ReverseMap();
    }
}
