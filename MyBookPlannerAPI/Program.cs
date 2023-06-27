using Microsoft.EntityFrameworkCore;
using MyBookPlannerAPI.Data;
using MyBookPlannerAPI.Services;
using System.Text;
using MyBookPlannerAPI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

var builder = WebApplication.CreateBuilder(args);

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
            // Se valida ou não a chave de assinatura
            ValidateIssuerSigningKey = true,

            // Como ele valida essa chave assinatura
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
});

ConfigureServices(builder);

builder.Services.AddControllers();
builder.Services.AddTransient<TokenService>();



var app = builder.Build();
app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();
void ConfigureServices(WebApplicationBuilder builder)
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<CatalogDataContext>(options => options.UseSqlServer(connectionString));
}


app.Run();
