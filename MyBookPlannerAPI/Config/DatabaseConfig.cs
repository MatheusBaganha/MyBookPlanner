using Microsoft.EntityFrameworkCore;
using MyBookPlanner.Repository.Data;

namespace MyBookPlanner.WebApi.Config
{
    public static class DatabaseConfig
    {
        public static IServiceCollection AddDatabase(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Get the current environment from the config and force uppercase
            var dbEnv = configuration["Database:Environment"]?.ToUpper()
                        ?? throw new Exception("Database:Environment is not configured");

            // Get the connection string for the selected environment
            var connectionString = configuration.GetConnectionString(dbEnv)
                ?? throw new Exception($"ConnectionString '{dbEnv}' not found");

            // Configure the DbContext with the correct connection string
            services.AddDbContext<MyBookPlannerDataContext>(options =>
                options.UseSqlite(connectionString));

            return services;
        }
    }
}
