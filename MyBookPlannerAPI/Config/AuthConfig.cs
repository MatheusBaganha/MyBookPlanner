using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyBookPlanner.Domain.Config;
using MyBookPlanner.Repository.Data;
using MyBookPlanner.Service.Services;
using MyBookPlanner.WebApi.Config.Requirements;

namespace MyBookPlanner.WebApi.Config
{
    public static class AuthConfig
    {
        public static IServiceCollection AddJwtAuthentication(
                 this IServiceCollection services,
                 IConfiguration configuration)
        {

            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

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


            services.AddAuthorization(options =>
            {
                // check if the user is doing the operation in his own account.
                options.AddPolicy("SameUser", policy =>
                        policy.Requirements.Add(new SameUserRequirement()));
            });

            return services;
        }
    }
}
