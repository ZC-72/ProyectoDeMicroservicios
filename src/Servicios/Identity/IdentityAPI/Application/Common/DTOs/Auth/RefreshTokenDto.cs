namespace Application.Common.DTOs.Auth;

public class RefreshTokenDto
{
    public string Token { get; set; }
    public DateTime Expires { get; set; }
    public DateTime Created { get; set; }
}