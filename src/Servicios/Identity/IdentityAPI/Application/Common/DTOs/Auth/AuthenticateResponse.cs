using System.Text.Json.Serialization;

namespace Application.Common.Models;

public class AuthenticateResponse
{
    public string AccessToken { get; set; }

    [JsonIgnore] // refreshToken es retornado en http only cookie.
    public string RefreshToken { get; set; }

    public AuthenticateResponse(string accessToken, string refreshToken)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }
}