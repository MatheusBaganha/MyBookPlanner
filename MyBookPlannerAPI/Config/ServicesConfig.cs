using MyBookPlanner.Service.Services;

namespace MyBookPlanner.WebApi.Config
{
    public static class ServicesConfig
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services)
        {
            services.AddControllers();
            services.AddHttpContextAccessor();

            // 1. Register OpenAPI with the Transformer
            services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
            });
            //services.AddOpenApi();
            services.AddEndpointsApiExplorer();
            
            return services;
        }
    }
}
