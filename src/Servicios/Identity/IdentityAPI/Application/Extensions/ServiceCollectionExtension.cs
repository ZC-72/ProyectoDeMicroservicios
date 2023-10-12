using Application.Common.Behaviours;
using Application.Common.Mappings;
using Application.Common.Validators.Auth;
using Application.Common.Validators.Users;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Application.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly));
        services.AddValidatorsFromAssemblyContaining<RegisterUserDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<LoginUserDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateUserInfoDtoValidator>();
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TaskCanceledExceptionBehaviour<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));


        //Configura mappings
        services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
        var mapperConfig = new MapperConfiguration(map =>
        {
            map.AddProfile<UserRegistrationMappingProfile>();
            map.AddProfile<UserMappingProfile>();
            map.AddProfile<UserRefreshTokenMappingProfile>();
        });
        services.AddSingleton(mapperConfig.CreateMapper());


        //Configura Swagger
        services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Por favor ingrese un token valido.",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }
}