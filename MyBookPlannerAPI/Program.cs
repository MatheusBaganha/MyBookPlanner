using Microsoft.EntityFrameworkCore;
using MyBookPlanner.Models;
using MyBookPlannerAPI.Data;

var builder = WebApplication.CreateBuilder(args);


ConfigureServices(builder);

builder.Services.AddControllers();


var app = builder.Build();
app.MapControllers();


void ConfigureServices(WebApplicationBuilder builder)
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<CatalogDataContext>(options => options.UseSqlServer(connectionString));
}


app.Run();
