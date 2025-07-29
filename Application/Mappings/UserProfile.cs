using AutoMapper;
using SalesSystem.Application.DTOs.User;
using SalesSystem.Domain.Entities;

namespace SalesSystem.Application.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<CreateUserRequest, User>();
    }
}