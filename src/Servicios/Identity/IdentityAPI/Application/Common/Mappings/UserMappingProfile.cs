using Application.Common.DTOs.User;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mappings;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<ApplicationUser, UserDto>();

        CreateMap<UpdateUserInfoRequest, ApplicationUser>().ReverseMap();
    }
}