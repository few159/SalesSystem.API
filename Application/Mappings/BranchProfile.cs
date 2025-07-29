using AutoMapper;
using SalesSystem.Application.DTOs.Branch;
using SalesSystem.Domain.Enitities.Snapshot;

namespace SalesSystem.Application.Mappings;

public class BranchProfile : Profile
{
    public BranchProfile()
    {
        CreateMap<BranchSnapshot, BranchDto>().ReverseMap();
    }
}
