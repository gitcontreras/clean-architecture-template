using Ecomm.Application.Abstractions.Persistence;
using Ecomm.Application.Products;
using Ecomm.Domain.Products;

namespace Ecomm.UnitTests.Application.Products;

public sealed class CreateProductHandlerTests
{
    [Fact]
    public async Task HandleAsyncPersistsAndReturnsProduct()
    {
        var repository = new SpyProductRepository();
        var handler = new CreateProductHandler(repository);

        var result = await handler.HandleAsync(new CreateProductCommand("Keyboard", 25));

        Assert.Equal("Keyboard", result.Name);
        Assert.Equal(25, result.Price);
        Assert.NotNull(repository.Product);
    }

    private sealed class SpyProductRepository : IProductRepository
    {
        public Product? Product { get; private set; }

        public Task<IReadOnlyCollection<Product>> ListAsync(CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyCollection<Product>>(
                Product is null ? [] : [Product]);

        public Task AddAsync(Product product, CancellationToken cancellationToken = default)
        {
            Product = product;
            return Task.CompletedTask;
        }
    }
}
