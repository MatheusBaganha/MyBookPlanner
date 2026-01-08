using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyBookPlanner.Repository.Data;
using MyBookPlanner.Service.Services;

namespace MyBookPlanner.WebApi.Config
{
    public static class AuthConfig
    {
        public static IServiceCollection AddJwtAuthentication(
                 this IServiceCollection services,
                 IConfiguration configuration)
        {
            var key = Encoding.ASCII.GetBytes(
                configuration["JwtSettings:Key"]
                ?? throw new Exception("JwtSettings:Key not configured")
            );

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero 
                    };
                });

            services.AddAuthorization();

            return services;
        }
    }
}
