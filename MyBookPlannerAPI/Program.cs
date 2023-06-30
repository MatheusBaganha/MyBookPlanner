using Microsoft.EntityFrameworkCore;
using MyBookPlannerAPI.Data;
using MyBookPlannerAPI.Services;
using System.Text;
using MyBookPlannerAPI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args);

ConfigureAuthentication(builder);
ConfigureServices(builder);

var app = builder.Build();

app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

app.UseHttpsRedirection();
app.Run();

void ConfigureServices(WebApplicationBuilder builder)
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<CatalogDataContext>(options => options.UseSqlServer(connectionString));

    builder.Services.AddControllers();
    builder.Services.AddTransient<TokenService>();
}

void ConfigureAuthentication(WebApplicationBuilder builder)
{
    // configuration is for us to have acess to the appsettings before the build of the app.
    var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

    Configuration.JwtKey = configuration.GetValue<string>("JwtKey");

    var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);

    builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }
        ).AddJwtBearer(x =>
        {
            x.TokenValidationParameters = new TokenValidationParameters
            {
                // Whether or not to validate the signature key.
                ValidateIssuerSigningKey = true,

                // How does it validate this signature key.
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });
}
