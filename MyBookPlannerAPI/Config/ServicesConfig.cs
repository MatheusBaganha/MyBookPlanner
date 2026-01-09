using MyBookPlanner.Service.Services;

namespace MyBookPlanner.WebApi.Config
{
    public static class ServicesConfig
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services)
        {
            services.AddControllers();
            services.AddTransient<TokenService>();


            services.AddOpenApi();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            return services;
        }
    }
}
