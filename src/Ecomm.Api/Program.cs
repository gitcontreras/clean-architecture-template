using Ecomm.Api.Common;
using Ecomm.Application;
using Ecomm.Infrastructure;
using Ecomm.Api.Extensions;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

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
}

app.MapControllers();

app.Run();

public partial class Program;
