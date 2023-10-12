using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IAccessTokenService
{
    Task<string> CreateAccessTokenAsync(ApplicationUser appUser);
}