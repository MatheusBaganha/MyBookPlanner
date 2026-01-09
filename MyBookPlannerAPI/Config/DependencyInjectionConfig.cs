using Microsoft.AspNetCore.Authorization;
using MyBookPlanner.Repository.Interfaces;
using MyBookPlanner.Repository.Repositorys;
using MyBookPlanner.Service.Interfaces;
using MyBookPlanner.Service.Services;
using MyBookPlanner.WebApi.Config.HandlerRequirements;

namespace MyBookPlanner.WebApi.Config
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddDIConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            // INJECTION ONLY ONCE (SINGLETON)
            #region
            services.AddSingleton<IAuthorizationHandler, SameUserHandler>();
            #endregion

            // INJECTION EVERY HTTP REQUEST (SCOPED)
            #region

            // SERVICES
            #region
            services.AddScoped<IBooksService, BooksService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserBooksService, UserBooksService>();
            services.AddScoped<IUserBooksStatisticsService, UserBooksStatisticsService>();
            services.AddScoped<IUserContextService, UserContextService>();
            #endregion

            // REPOSITORYS
            #region
            services.AddScoped<IBooksRepository, BooksRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserBooksRepository, UserBooksRepository>();
            services.AddScoped<IGenericRepository, GenericRepository>();

            #endregion

            #endregion

            // INJECTION EVERY TIME THE SERVICE IS CALLED  (TRANSIENT)
            #region
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<ITokenService, TokenService>();

            #endregion


            services.AddHttpContextAccessor();


            return services;
        }
    }
}
