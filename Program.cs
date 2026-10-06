using Scalar.AspNetCore;
using WebApiAutores.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<AutorService>();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var numbersService = scope.ServiceProvider.GetRequiredService<AutorService>();
    await numbersService.InitializeAsync();
}

app.MapOpenApi();
app.MapScalarApiReference();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
