using Application.Common.DTOs.Auth;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Wrappers;

namespace Application.Commands.Auth;

public record LoginUserCommand(LoginUserRequest loginUserRequest) : IRequestWrapper<AuthenticateResponse>;

internal sealed class LoginUserCommandHandler : IHandlerWrapper<LoginUserCommand, AuthenticateResponse>
{
    protected readonly IUserAuthenticationService _userAuthService;

    public LoginUserCommandHandler(IUserAuthenticationService userAuthService) =>
     _userAuthService = userAuthService;

    public async Task<IResponse<AuthenticateResponse>> Handle(
        LoginUserCommand request,
        CancellationToken cancellationToken) =>
            Response.Success(await _userAuthService.Authenticate(request));
}