using System;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Students.Api.Infrastructure.Security.Auth;
using Students.Api.Settings;

namespace Students.Api.Extensions
{
  public static class ServiceCollectionExtensions
  {
    public static IServiceCollection AddCustomJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
      var jwtTokenConfig = configuration.GetSection(Consts.Configs.JwtToken).Get<JwtTokenConfig>();

      services
        .AddSingleton(jwtTokenConfig)
        .AddSingleton<IJwtAuthManager, JwtAuthManager>()
        .AddHostedService<JwtRefreshTokenHostedService>()
        .AddAuthentication(options =>
        {
          options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
          options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
          options.RequireHttpsMetadata = true;
          options.SaveToken = true;
          options.TokenValidationParameters = new TokenValidationParameters
          {
            ValidateIssuer = true,
            ValidIssuer = jwtTokenConfig.Issuer,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtTokenConfig.Secret)),
            ValidAudience = jwtTokenConfig.Audience,
            ValidateAudience = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
          };
        });

      return services;
    }

    public static IServiceCollection AddCustomAuthorizations(this IServiceCollection services)
    {
      return services
        .AddUsersAuthorizations();
    }

    public static IServiceCollection AddUsersAuthorizations(this IServiceCollection services)
    {
      return services
        .AddAuthorization(opt => opt.AddPolicy(nameof(CustomClaimTypes.UserId),
          policy => policy.RequireClaim(CustomClaimTypes.UserId)));
    }

    public static IServiceCollection AddCustomSwaggerGen(this IServiceCollection services)
    {
      services.AddSwaggerGen(options =>
      {
        //options.DescribeAllParametersInCamelCase();
        //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        //options.IncludeXmlComments(xmlPath);
        //options.EnableAnnotations();

        var securityScheme = new OpenApiSecurityScheme
        {
          Reference = new OpenApiReference
          {
            Type = ReferenceType.SecurityScheme,
            Id = JwtBearerDefaults.AuthenticationScheme,
          },
          Description = "JWT Authorization header using the Bearer scheme",
          In = ParameterLocation.Header,
          Name = "Authorization",
          Type = SecuritySchemeType.Http,
          Scheme = JwtBearerDefaults.AuthenticationScheme,
          BearerFormat = "JWT",
        };

        options.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);

        options.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
            {securityScheme, Array.Empty<string>()}
        });
      });
      return services;
    }
  }
}
