using Microsoft.AspNetCore.Builder;
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

if (app.Environment.IsDevelopment())
{
    // Expose the OpenAPI document (default at /openapi/v1.json)
    app.MapOpenApi();

    // Enable Swagger UI
    app.UseSwaggerUI(options =>
    {
        // Point to the .NET 10 OpenAPI document endpoint
        options.SwaggerEndpoint("/openapi/v1.json", "My API v1");
    });
}


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




