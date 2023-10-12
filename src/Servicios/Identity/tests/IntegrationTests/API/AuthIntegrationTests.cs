using Application.Common.DTOs.Auth;
using Application.Common.Models;
using System.Net.Http.Json;

using IntegrationTests.Common;

namespace IntegrationTests.API;

public class AuthIntegrationTests : BaseClassFixture
{
    public AuthIntegrationTests(CustomWebApplicationFactory<Program> customWebApplicationFactory)
        : base(customWebApplicationFactory) { }

    [Fact]
    public async Task Register_ErrorWhenPassInValidData()
    {
        // Arrenge
        const string requestUri = "v1/userauthentication/register";
        var request = new RegisterUserRequest
        {
            FirstName = "a",
            LastName = "a",
            UserName = "a",
            Password = "1",
            ConfirmPassword = "12",
            Email = "a"
        };

        // Act
        var response = await Client.PostAsJsonAsync(requestUri, request);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task Register_SuccessWhenPassValidData()
    {
        // Arrenge
        const string requestUri = "v1/userauthentication/register";
        var request = new RegisterUserRequest
        {
            FirstName = "abc",
            LastName = "abc",
            UserName = "usuario",
            Password = "123456",
            ConfirmPassword = "123456",
            Email = "usuario@email"
        };

        // Act
        var response = await Client.PostAsJsonAsync(requestUri, request);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Contains("Usuario registrado con éxito.", responseString);
    }



    [Fact]
    public async Task Login_ErrorWhenPassInValidData()
    {
        // Arrenge
        const string requestUri = "v1/userauthentication/login";
        var request = new LoginUserRequest
        {
            UserName = "abc",
            Password = "123456",
        };

        // Act
        var response = await Client.PostAsJsonAsync(requestUri, request);
        var responseString = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Contains("incorrectos.", responseString);
    }

    [Fact]
    public async Task Login_SuccessWhenPassValidData()
    {
        // Arrenge
        const string requestUri = "v1/userauthentication/login";
        var request = new LoginUserRequest
        {
            UserName = "usuario",
            Password = "123456",
        };

        // Act
        var response = await Client.PostAsJsonAsync(requestUri, request);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Contains("Inicio de sesión exitoso.", responseString);
    }


    [Fact]
    public async Task Generate_New_Token_ErrorWhenPassInValidCookieData()
    {
        // Arrenge
        const string refreshUri = "v1/userauthentication/new-token";

        // Act
        var refreshResponse = await Client.PostAsJsonAsync(refreshUri, "");
        var responseString = await refreshResponse.Content.ReadAsStringAsync();

        // Assert
        Assert.False(refreshResponse.IsSuccessStatusCode);
        Assert.Contains("El token no es valido.", responseString);
    }

    [Fact]
    public async Task Generate_New_Token_SuccessWhenPassValidCookieData()
    {
        // Arrenge
        const string loginUri = "v1/userauthentication/login";
        const string refreshUri = "v1/userauthentication/new-token";

        var loginRequest = new LoginUserRequest
        {
            UserName = "usuario",
            Password = "123456",
        };

        // Act
        var response = await Client.PostAsJsonAsync(loginUri, loginRequest);
        response.EnsureSuccessStatusCode();
        var refreshResponse = await Client.PostAsJsonAsync(refreshUri, "");
        refreshResponse.EnsureSuccessStatusCode();
        var responseString = await refreshResponse.Content.ReadAsStringAsync();

        // Assert
        Assert.Contains("Nuevo token generado con éxito.", responseString);
    }



    [Fact]
    public async Task Revoke_Refresh_Token_ErrorWhenPassInValidData()
    {
        // Arrenge
        const string revokeUri = "v1/userauthentication/revoke-token";
        var tokenToRevoke = new RevokeTokenRequest { Token = "" };

        // Act
        var refreshResponse = await Client.PostAsJsonAsync(revokeUri, tokenToRevoke);
        var responseString = await refreshResponse.Content.ReadAsStringAsync();

        // Assert
        Assert.False(refreshResponse.IsSuccessStatusCode);
        Assert.Contains("El token no es valido.", responseString);
    }

    [Fact]
    public async Task Revoke_Refresh_Token_ErrorWhenPassInValidRequestBodyData()
    {
        // Arrenge
        const string loginUri = "v1/userauthentication/login";
        var loginRequest = new LoginUserRequest
        {
            UserName = "usuario",
            Password = "123456",
        };

        const string revokeUri = "v1/userauthentication/revoke-token";
        var tokenToRevoke = new RevokeTokenRequest { Token = "" };

        // Act
        var loginResponse = await Client.PostAsJsonAsync(loginUri, loginRequest);
        loginResponse.EnsureSuccessStatusCode();
        var revokeResponse = await Client.PostAsJsonAsync(revokeUri, tokenToRevoke);
        var responseString = await revokeResponse.Content.ReadAsStringAsync();

        // Assert
        Assert.False(revokeResponse.IsSuccessStatusCode);
        Assert.Contains("El token no es valido.", responseString);
    }

    [Fact]
    public async Task Revoke_Refresh_Token_SuccessWhenPassValidCookieData()
    {
        // Arrenge
        const string loginUri = "v1/userauthentication/login";
        var loginRequest = new LoginUserRequest
        {
            UserName = "usuario",
            Password = "123456",
        };

        const string revokeUri = "v1/userauthentication/revoke-token";
        var tokenToRevoke = new RevokeTokenRequest { Token = null };

        // Act
        var loginResponse = await Client.PostAsJsonAsync(loginUri, loginRequest);
        loginResponse.EnsureSuccessStatusCode();
        var revokeResponse = await Client.PostAsJsonAsync(revokeUri, tokenToRevoke);
        revokeResponse.EnsureSuccessStatusCode();
        var responseString = await revokeResponse.Content.ReadAsStringAsync();

        // Assert
        Assert.Contains("Token revocado con éxito.", responseString);
    }

    [Fact]
    public async Task Revoke_Refresh_Token_SuccessWhenPassValidRequestBodyData()
    {
        // Arrenge
        const string revokeUri = "v1/userauthentication/revoke-token";
        var tokenToRevoke = new RevokeTokenRequest
        {
            Token = "TokenVigenteARevocar"
        };

        // Act
        var revokeResponse = await Client.PostAsJsonAsync(revokeUri, tokenToRevoke);
        revokeResponse.EnsureSuccessStatusCode();
        var responseString = await revokeResponse.Content.ReadAsStringAsync();

        // Assert
        Assert.Contains("Token revocado con éxito.", responseString);
    }
}