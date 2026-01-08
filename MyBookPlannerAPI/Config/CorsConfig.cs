namespace MyBookPlanner.WebApi.Config
{
    public static class CorsConfig
    {
        public static IServiceCollection AddCorsConfiguration(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("MyBookPlannerPolicy",
                policy => policy.AllowAnyOrigin()
                                  .AllowAnyHeader()
                                  .AllowAnyMethod());
            });

            return services;
        }
    }
}
