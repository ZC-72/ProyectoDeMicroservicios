using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IRefreshTokenService
{
    RefreshToken CreateRefreshToken();
}