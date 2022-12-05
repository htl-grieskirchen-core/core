using Core.AuthLib.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Core.AuthLib;

public static class AuthConfigurator
{

    /// <summary>
    /// Only to use in Core.Backend & Core.BackendSecure!
    /// </summary>
    public static void AddCookieAuth(this WebApplicationBuilder builder, bool isPrivateKey = false)
    {
        builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearerExt(builder, true, isPrivateKey);

        builder.AddAuthorizationPolicies();
    }

    public static void AddHeaderAuth(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearerExt(builder);

        builder.AddAuthorizationPolicies();
    }

    private static void AddJwtBearerExt(this AuthenticationBuilder authBuilder, WebApplicationBuilder builder, bool isCookie = false, bool isPrivateKey = false)
    {
        authBuilder.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            options.RequireHttpsMetadata = true;
            options.SaveToken = true;
            options.TokenValidationParameters = ValidationParameters(builder, isPrivateKey);

            if (isCookie)
            {
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["auth"];
                        return Task.CompletedTask;
                    }
                };
            }
        });
    }

    public static void AddSwaggerGenHeader(this SwaggerGenOptions option)
    {
        option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
        });
        option.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
            }
        });
    }

    private static void AddAuthorizationPolicies(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorization(opt =>
        {
            opt.AddPolicy("ID-Token", policy => policy.RequireClaim("type", "id-token"));
            opt.AddPolicy("Auth-Token", policy => policy.RequireClaim("type", "auth-token"));
        });
    }

    private static TokenValidationParameters ValidationParameters(WebApplicationBuilder builder, bool isPrivateKey = false) =>
        new()
        {
            ClockSkew = TimeSpan.Zero,
            ValidateLifetime = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateIssuerSigningKey = true,
            // TODO: deduplicate
            IssuerSigningKey = new RsaSecurityKey(RsaService.ImportRSAKey("./keys/" + builder.Configuration[!isPrivateKey ? "RSA:public-key" : "RSA:private-key"], isPrivateKey))
        };
}
