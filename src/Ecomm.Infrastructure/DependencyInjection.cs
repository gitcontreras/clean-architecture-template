using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.DependencyInjection;
using Ecomm.Application.Abstractions.Persistence;
using Ecomm.Application.Products;
using Ecomm.Infrastructure.Persistence;

namespace Ecomm.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IProductRepository, InMemoryProductRepository>();
        services.AddScoped<CreateProductHandler>();
        services.AddScoped<ListProductsHandler>();
        services.AddScoped<IDbConnectionFactory, SqlServerConnectionFactory>();
        return services;
    }
}
