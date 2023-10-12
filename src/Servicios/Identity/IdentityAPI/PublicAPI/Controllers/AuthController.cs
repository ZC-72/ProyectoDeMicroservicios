using Application.Commands.Auth;
using Application.Common.DTOs.Auth;
using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.Controllers;

[Route("v1/userauthentication")]
[ApiController]
public class AuthController : BaseApiController
{
    private readonly IValidator<RegisterUserRequest> _registerValidator;
    private readonly IValidator<LoginUserRequest> _loginValidator;

    public AuthController(
        IMediator mediator,
        ILoggerManager logger,
        IValidator<RegisterUserRequest> registerValidator,
        IValidator<LoginUserRequest> loginValidator) : base(mediator, logger)
    {
        _registerValidator = registerValidator;
        _loginValidator = loginValidator;
    }



    /// <summary>
    /// Registra un nuevo usuario.
    /// </summary>
    /// <param name="userToCreate">Los datos del usuario a registrar.</param>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<IActionResult> RegisterUser([FromBody]
     RegisterUserRequest userToCreate,
     CancellationToken cancellationToken = new())
    {
        var validationResult = await _registerValidator.ValidateAsync(userToCreate, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(
                validationResult.Errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(
                  g => g.Key,
                  g => g.Select(x => x.ErrorMessage).ToArray()
                ));

        var result = await _mediator.Send(new RegisterUserCommand(userToCreate), cancellationToken);
        return !result.Data.Succeeded ?
                BadRequest(result) :
                Ok(new { message = "Usuario registrado con éxito.", result });
    }



    /// <summary>
    /// Inicia sesión de usuario. También provee access token y refresh token.
    /// </summary>
    /// <param name="loginUserRequest">Los datos del usuario que desea iniciar sesión.</param>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<ActionResult<AuthenticateResponse>> Authenticate([FromBody]
     LoginUserRequest loginUserRequest,
     CancellationToken cancellationToken = new())
    {
        var validationResult = await _loginValidator.ValidateAsync(loginUserRequest, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(
                validationResult.Errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(
                  g => g.Key,
                  g => g.Select(x => x.ErrorMessage).ToArray()
                ));

        var result = await _mediator.Send(new LoginUserCommand(loginUserRequest), cancellationToken);
        SetTokenCookie(result.Data.RefreshToken);
        return !result.Succeeded ?
                BadRequest(result) :
                Ok(new { message = "Inicio de sesión exitoso.", result });
    }



    /// <summary>
    /// Refresca tokens.(access token y refresh token).
    /// </summary>
    [HttpPost("new-token")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<ActionResult<AuthenticateResponse>> NewTokens(CancellationToken cancellationToken = new())
    {
        var refreshToken = Request.Cookies["refreshToken"];
        var result = await _mediator.Send(new NewTokenCommand(refreshToken), cancellationToken);
        SetTokenCookie(result.Data.RefreshToken);
        return !result.Succeeded ?
                BadRequest(result) :
                Ok(new { message = "Nuevo token generado con éxito.", result });
    }


    /// <summary>
    /// Revoca refresh tokens.
    /// </summary>
    /// <param name="request">Petición que contiene el refresh token a revocar.</param>
    [HttpPost("revoke-token")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Produces("application/json")]
    [Consumes("application/json")]
    public async Task<IActionResult> RevokerRefreshToken(RevokeTokenRequest request, CancellationToken cancellationToken = new())
    {
        // Acepta un refresh token en el cuerpo de la petición o en una cookie.
        var token = request.Token ?? Request.Cookies["refreshToken"];
        var result = await _mediator.Send(new RebokeTokenCommand(token), cancellationToken);
        return !result.Succeeded ?
                BadRequest(result) :
                Ok(new { message = "Token revocado con éxito.", result });
    }


    //Helper method

    // Adjunta una cookie con un refresh token a la respuesta.
    private void SetTokenCookie(string token)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(7)
        };
        Response.Cookies.Append("refreshToken", token, cookieOptions);
    }
}
