using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Wrappers;
using MediatR;

namespace Application.Commands.Auth;

public record RebokeTokenCommand(string token) : IRequestWrapper<Unit>;

internal sealed class RebokeTokenCommandHandler : IHandlerWrapper<RebokeTokenCommand, Unit>
{
    protected readonly IUserAuthenticationService _userAuthService;

    public RebokeTokenCommandHandler(IUserAuthenticationService userAuthService) =>
    _userAuthService = userAuthService;

    public async Task<IResponse<Unit>> Handle(
        RebokeTokenCommand request,
         CancellationToken cancellationToken) =>
            Response.Success(await _userAuthService.RevokeRefreshToken(request));
}