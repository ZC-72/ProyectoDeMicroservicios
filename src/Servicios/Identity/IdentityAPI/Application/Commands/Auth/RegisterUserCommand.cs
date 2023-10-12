using Application.Common.DTOs.Auth;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Wrappers;
using Microsoft.AspNetCore.Identity;

namespace Application.Commands.Auth;

public record RegisterUserCommand(RegisterUserRequest registerUserRequest) : IRequestWrapper<IdentityResult>;

internal sealed class RegisterUserCommandHandler : IHandlerWrapper<RegisterUserCommand, IdentityResult>
{
    protected readonly IUserAuthenticationService _userAuthService;

    public RegisterUserCommandHandler(IUserAuthenticationService userAuthService) =>
    _userAuthService = userAuthService;

    public async Task<IResponse<IdentityResult>> Handle(
        RegisterUserCommand request,
        CancellationToken cancellationToken) =>
            Response.Success(await _userAuthService.RegisterUserAsync(request));
}