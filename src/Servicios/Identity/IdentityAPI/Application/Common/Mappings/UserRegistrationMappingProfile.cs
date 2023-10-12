using Application.Common.DTOs.Auth;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mappings;

public class UserRegistrationMappingProfile : Profile
{
    public UserRegistrationMappingProfile()
    {
        CreateMap<RegisterUserRequest, ApplicationUser>();
    }
}
