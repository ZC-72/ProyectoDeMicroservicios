using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Wrappers;

namespace Application.Commands.Auth;

public record NewTokenCommand(string token) : IRequestWrapper<AuthenticateResponse>;

internal sealed class NewTokenCommandHandler : IHandlerWrapper<NewTokenCommand, AuthenticateResponse>
{
    protected readonly IUserAuthenticationService _userAuthService;

    public NewTokenCommandHandler(IUserAuthenticationService userAuthService) =>
    _userAuthService = userAuthService;

    public async Task<IResponse<AuthenticateResponse>> Handle(
        NewTokenCommand request,
        CancellationToken cancellationToken) =>
            Response.Success(await _userAuthService.GenerateNewTokens(request));
}