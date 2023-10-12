using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace IntegrationTests.Common;

public static class JwtTokenProvider
{
    public static string Issuer { get; } = "Testing_Auth_Server";
    public static SecurityKey SecurityKey { get; } =
        new SymmetricSecurityKey(
            Encoding.ASCII.GetBytes("SuperSecretKeyUtilizadaEnEstosCasos")
        );
    public static SigningCredentials SigningCredentials { get; } =
        new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);
    internal static readonly JwtSecurityTokenHandler JwtSecurityTokenHandler = new();
}