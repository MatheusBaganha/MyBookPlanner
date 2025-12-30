using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyBookPlanner.Domain.Config;
using MyBookPlanner.Repository.Data;
using MyBookPlanner.Service.Services;


var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

ConfigureAuthentication(builder);
ConfigureServices(builder);

var app = builder.Build();

app.MapControllers();

app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.Run();

void ConfigureServices(WebApplicationBuilder builder)
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<MyBookPlannerDataContext>(options => options.UseSqlite(connectionString));

    builder.Services.AddControllers();
    builder.Services.AddTransient<TokenService>();
}



void ConfigureAuthentication(WebApplicationBuilder builder)
{
    // Get the key directly from appsettings.json via the IOptions pattern.
    var key = Encoding.ASCII.GetBytes(builder.Configuration["JwtSettings:Key"]);

    builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(x =>
    {
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
}
