using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;


namespace IntegrationTests.Common;

public class CustomWebApplicationFactory<TEntryPoint> : WebApplicationFactory<Program> where TEntryPoint : Program
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        base.ConfigureWebHost(builder);
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.Configure<JwtBearerOptions>(
                JwtBearerDefaults.AuthenticationScheme,
                    options =>
                    {
                        options.Configuration = new OpenIdConnectConfiguration
                        {
                            Issuer = JwtTokenProvider.Issuer,
                        };
                        options.TokenValidationParameters.ValidIssuer = JwtTokenProvider.Issuer;
                        options.TokenValidationParameters.ValidAudience = JwtTokenProvider.Issuer;
                        options.Configuration.SigningKeys.Add(JwtTokenProvider.SecurityKey);
                    }
            );
        });

        return base.CreateHost(builder);
    }
}