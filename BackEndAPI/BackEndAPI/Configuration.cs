// Ignore Spelling: Redis, jwt
using Domain;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using UseCase;

namespace BackEndAPI
{
    public static class Configuration
    {
        public static IServiceCollection AddDependencies(
          this IServiceCollection services,
          IConfiguration configuration)
        {
            // Layers Dependencies
            services.AddCheckUserSecrets(configuration);
            services.AddDomainConfiguration();
            services.AddUseCaseConfiguration();
            services.AddInfrastructureConfiguration(configuration);

            // JWT configuration
            services.AddAuthentication(opt =>
                {
                    //Creating Default Scheme [We can use in different Controllers Different Scheme]
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(opt =>
                {
                    opt.SaveToken = true;
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateLifetime = true, // ClockSkew
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,

                        ClockSkew = TimeSpan.Zero, // Allowed Expired Tokens,ex. TimeSpan.FromMinutes(1)
                        ValidIssuer = UseCase.Configuration.JwtIssuer, // Who Gives Token
                        ValidAudience = UseCase.Configuration.JwtAudience, //  Who Given Token
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(UseCase.Configuration.JwtSecret)
                            ),
                    };

                    // Returns info about Expired Token
                    opt.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.Headers.Append("Token-expired", "true");
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            // Swagger with JWT configuration
            services.AddSwaggerGen(opt =>
            {
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter 'Bearer [jwt]'",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                });

                var scheme = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { scheme, Array.Empty<string>() }
                });
            });

            return services;
        }
    }
}
