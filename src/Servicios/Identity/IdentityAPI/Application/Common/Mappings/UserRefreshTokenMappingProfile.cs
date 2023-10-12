using Application.Common.DTOs.Auth;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mappings;

public class UserRefreshTokenMappingProfile : Profile
{
    public UserRefreshTokenMappingProfile()
    {
        CreateMap<RefreshToken, RefreshTokenDto>();
    }
}