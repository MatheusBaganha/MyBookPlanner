using Microsoft.EntityFrameworkCore;
using MyBookPlanner.Repository.Data;
using MyBookPlanner.WebApi.Config;


var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services
    .AddDatabase(builder.Configuration)
    .AddApplicationServices()
    .AddCorsConfiguration()
    .AddJwtAuthentication(builder.Configuration);

var app = builder.Build();


// Middleware
app.UseHttpsRedirection();

// Name of the policy configured
app.UseCors("MyBookPlannerPolicy");

app.UseAuthentication();
app.UseAuthorization();


// Endpoints (always at last)
app.MapControllers();


// To make things easier since it's just a side project, those books will be inserted in this generic way.
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MyBookPlannerDataContext>();
    context.Database.EnsureCreated(); // Ensures that the database and tables exist.
    DbSeeder.SeedBooks(context);  // Populate the books
}

app.Run();




