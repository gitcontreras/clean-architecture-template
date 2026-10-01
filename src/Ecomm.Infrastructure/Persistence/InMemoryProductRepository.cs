using System.Collections.Concurrent;
using Ecomm.Application.Abstractions.Persistence;
using Ecomm.Domain.Products;

namespace Ecomm.Infrastructure.Persistence;

internal sealed class InMemoryProductRepository : IProductRepository
{
    private readonly ConcurrentDictionary<Guid, Product> products = new();

    public Task<IReadOnlyCollection<Product>> ListAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        IReadOnlyCollection<Product> result = products.Values.ToArray();
        return Task.FromResult(result);
    }

    public Task AddAsync(Product product, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        products[product.Id] = product;
        return Task.CompletedTask;
    }
}
