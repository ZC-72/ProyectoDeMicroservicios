using System.Text.Json;
using Domain.Exceptions;

namespace PublicAPI.CustomExceptionMiddleware;

/// <summary>
/// Maneja todas las excepciones del microservicio.
/// </summary>
internal sealed class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) => _logger = logger;
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected error.");
            await HandleExceptionAsync(context, e);
        }
    }


    /// <summary>
    /// Obtiene el HTTP status code y el mensaje correspondiente a la excepción.
    /// </summary>
    /// <param name="exception">La excepción.</param>
    /// <returns>Retorna el status code y el mensaje al caller.</returns>
    private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        var statusCode = GetStatusCode(exception);
        var response = new
        {
            status = statusCode,
            message = exception.Message
        };
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
    private static int GetStatusCode(Exception exception) =>
        exception switch
        {
            BadRequestException or InvalidRefreshTokenException => StatusCodes.Status400BadRequest,
            UserNotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };
}