using Application.Common.DTOs.User;
using IntegrationTests.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;

namespace IntegrationTests.API;

public class UsersIntegrationTests : BaseClassFixture
{
    public UsersIntegrationTests(CustomWebApplicationFactory<Program> customWebApplicationFactory)
        : base(customWebApplicationFactory) { }

    [Fact]
    public async Task Get_All_Users_ErrorWhenUserIsAnonymous()
    {
        // Arrenge
        const string requestUri = "v1/users";

        // Act
        var response = await Client.GetAsync(requestUri);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Get_All_Users_ErrorWhenPassInValidJwtTokenClaims()
    {
        // Arrenge
        var accessToken = GenerateModeratorAccesToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        const string requestUri = "v1/users";

        // Act
        var response = await Client.GetAsync(requestUri);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task Get_All_Users_SuccessWhenPassValidJwtToken()
    {
        // Arrenge
        var accessToken = GenerateSuperAdminAccesToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        const string requestUri = "v1/users";

        // Act
        var response = await Client.GetAsync(requestUri);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Contains("superadmin", responseString);
    }




    [Fact]
    public async Task Get_User_By_Id_ErrorWhenUserIsAnonymous()
    {
        // Arrenge
        var userId = "UserIdExitente";
        var requestUri = "v1/users/" + userId;

        // Act
        var response = await Client.GetAsync(requestUri);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Get_User_By_Id_ErrorWhenPassInValidJwtTokenClaims()
    {
        // Arrenge
        var accessToken = GenerateModeratorAccesToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var userId = "UserIdExitente";
        var requestUri = "v1/users/" + userId;

        // Act
        var response = await Client.GetAsync(requestUri);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task Get_User_By_Id_ErrorWhenPassInValidData()
    {
        // Arrenge
        var accessToken = GenerateSuperAdminAccesToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var userId = "UserIdInexitente";
        var requestUri = "v1/users/" + userId;

        // Act
        var response = await Client.GetAsync(requestUri);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Get_User_By_Id_SuccessWhenPassValidJwtTokenAndValidUserId()
    {
        // Arrenge
        var accessToken = GenerateSuperAdminAccesToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var userId = "UserIdExitente";
        var requestUri = "v1/users/" + userId;

        // Act
        var response = await Client.GetAsync(requestUri);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Contains("UserIdExitente", responseString);
    }



    [Fact]
    public async Task Get_All_Refresh_Tokens_By_User_Id_ErrorWhenUserIsAnonymous()
    {
        // Arrenge
        var userId = "UserIdExitente";
        var requestUri = "v1/users/" + userId + "/refresh-tokens-by-user-id/";

        // Act
        var response = await Client.GetAsync(requestUri);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Get_All_Refresh_Tokens_By_User_Id_ErrorWhenPassInValidJwtTokenClaims()
    {
        // Arrenge
        var accessToken = GenerateModeratorAccesToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var userId = "UserIdExitente";
        var requestUri = "v1/users/" + userId + "/refresh-tokens-by-user-id/";

        // Act
        var response = await Client.GetAsync(requestUri);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task Get_All_Refresh_Tokens_By_User_Id_ErrorWhenPassInValidData()
    {
        // Arrenge
        var accessToken = GenerateSuperAdminAccesToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var userId = "UserIdInexitente";
        var requestUri = "v1/users/" + userId + "/refresh-tokens-by-user-id/";

        // Act
        var response = await Client.GetAsync(requestUri);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Get_All_Refresh_Tokens_By_User_Id_SuccessWhenPassValidJwtTokenAndValidUserId()
    {
        // Arrenge
        var accessToken = GenerateSuperAdminAccesToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var userId = "UserIdExitente";
        var requestUri = "v1/users/" + userId + "/refresh-tokens-by-user-id/";

        // Act
        var response = await Client.GetAsync(requestUri);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Contains("Token", responseString);
    }


    [Fact]
    public async Task Update_User_ErrorWhenUserIsAnonymous()
    {
        // Arrenge
        var userId = "UserIdExitente";
        var requestUri = "v1/users/" + userId;

        // Act
        var response = await Client.PutAsJsonAsync(requestUri, "");

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Update_User_ErrorWhenPassInValidJwtTokenClaims()
    {
        // Arrenge
        var accessToken = GenerateModeratorAccesToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var userId = "UserIdExitente";
        var requestUri = "v1/users/" + userId;

        // Act
        var response = await Client.PutAsJsonAsync(requestUri, "");

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task Update_User_ErrorWhenPassInValidData()
    {
        // Arrenge
        var accessToken = GenerateSuperAdminAccesToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var userId = "UserIdInexitente";
        var request = new UpdateUserInfoRequest
        {
            FirstName = "a",
            LastName = "a",
            UserName = "a",
            Email = "a"
        };
        var requestUri = "v1/users/" + userId;

        // Act
        var response = await Client.PutAsJsonAsync(requestUri, request);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Update_User_ErrorWhenPassInValidUserId()
    {
        // Arrenge
        var accessToken = GenerateSuperAdminAccesToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var userId = "UserIdInexitente";
        var request = new UpdateUserInfoRequest
        {
            FirstName = "nuevo Nombre",
            LastName = "nuevo Apellido",
            UserName = "NuevoUsuario",
            Email = "nuevoUsuario@email"
        };
        var requestUri = "v1/users/" + userId;

        // Act
        var response = await Client.PutAsJsonAsync(requestUri, request);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Update_User_SuccessWhenPassValidUserIdAndValidData()
    {
        // Arrenge
        var accessToken = GenerateSuperAdminAccesToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var userId = "UserIdExitente";
        var request = new UpdateUserInfoRequest
        {
            FirstName = "nuevo Nombre",
            LastName = "nuevo Apellido",
            UserName = "NuevoUsuario777",
            Email = "nuevoUsuario777@email"
        };
        var requestUri = "v1/users/" + userId;

        // Act
        var response = await Client.PutAsJsonAsync(requestUri, request);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Contains("Usuario actualizado con éxito.", responseString);
    }


    //Helper Methods

    private static string GenerateSuperAdminAccesToken()
    {
        var accesToken = JwtTokenProvider.JwtSecurityTokenHandler.WriteToken(
           new JwtSecurityToken(
               JwtTokenProvider.Issuer,
               JwtTokenProvider.Issuer,
               new List<Claim> { new(ClaimTypes.Role, "SuperAdmin"), },
               expires: DateTime.Now.AddMinutes(15),
               signingCredentials: JwtTokenProvider.SigningCredentials
           )
       );
        return accesToken;
    }

    private static string GenerateModeratorAccesToken()
    {
        var accesToken = JwtTokenProvider.JwtSecurityTokenHandler.WriteToken(
           new JwtSecurityToken(
               JwtTokenProvider.Issuer,
               JwtTokenProvider.Issuer,
               new List<Claim> { new(ClaimTypes.Role, "Moderator"), },
               expires: DateTime.Now.AddMinutes(15),
               signingCredentials: JwtTokenProvider.SigningCredentials
           )
       );
        return accesToken;
    }
}