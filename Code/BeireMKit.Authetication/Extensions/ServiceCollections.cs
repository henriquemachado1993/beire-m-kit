using BeireMKit.Authetication.Interfaces.Jwt;
using BeireMKit.Authetication.Models;
using BeireMKit.Authetication.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BeireMKit.Authetication.Extensions
{
    public static class ServiceCollections
    {
        public static IServiceCollection ConfigureJwtAuthentication(this IServiceCollection services, JwtSettings jwtSettings)
        {
            if (string.IsNullOrWhiteSpace(jwtSettings.SecretKey) ||
               string.IsNullOrWhiteSpace(jwtSettings.Issuer) ||
               string.IsNullOrWhiteSpace(jwtSettings.Audience))
            {
                throw new ArgumentNullException(nameof(jwtSettings));
            }

            services.AddSingleton(jwtSettings);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build());
            });

            return services;
        }

        public static IServiceCollection ConfigureJwtAuthentication(this IServiceCollection services,
            TokenValidationParameters tokenValidationParameters,
            Action<AuthenticationOptions> configureAuthenticationOptions,
            Action<AuthorizationOptions> configureAuthorizationOptions
            )
        {
            if (tokenValidationParameters == null)
                throw new ArgumentNullException(nameof(tokenValidationParameters));

            if (configureAuthenticationOptions == null)
                throw new ArgumentNullException(nameof(configureAuthenticationOptions));

            if (configureAuthorizationOptions == null)
                throw new ArgumentNullException(nameof(configureAuthorizationOptions));

            services.AddAuthentication(configureAuthenticationOptions)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = tokenValidationParameters;
                });
            services.AddAuthorization(configureAuthorizationOptions);
            return services;
        }

        public static IServiceCollection ConfigureJwtServices(this IServiceCollection services)
        {
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IPasswordHasherService, PasswordHasherService>();
            return services;
        }
    }
}
