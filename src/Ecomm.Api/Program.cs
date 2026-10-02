using Ecomm.Api.Common;
using Ecomm.Application;
using Ecomm.Infrastructure;
using Ecomm.Api.Extensions;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
// CORS temporal para desarrollo local: permite orígenes desde localhost/127.0.0.1
builder.Services.AddCors(options =>
{
    options.AddPolicy("LocalDevCors", policy => policy
        .SetIsOriginAllowed(origin => origin != null && (
            origin.StartsWith("http://localhost", System.StringComparison.OrdinalIgnoreCase) || origin.StartsWith("https://localhost", System.StringComparison.OrdinalIgnoreCase) ||
            origin.StartsWith("http://127.0.0.1", System.StringComparison.OrdinalIgnoreCase) || origin.StartsWith("https://127.0.0.1", System.StringComparison.OrdinalIgnoreCase)))
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials());
});

builder.Services.AddControllers();
builder.Services.AddSwaggerDocumentation();
builder.Services.AddInfrastructure();
// Registrar automáticamente repositorios/servicios generados por convención
builder.Services.AddGeneratedServices(
    ServiceLifetime.Scoped,
    typeof(ApplicationAssemblyMarker).Assembly,
    typeof(InfrastructureAssemblyMarker).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();
    // Aplicar CORS local durante desarrollo
    app.UseCors("LocalDevCors");
}

app.MapControllers();

app.Run();

public partial class Program;
