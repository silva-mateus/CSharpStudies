using IX10_MinimalAPI_ProductCatalog.Models;
using System.Collections.Concurrent;

namespace IX10_MinimalAPI_ProductCatalog.Repositories;

public class InMemoryProductRepository : IProductRepository
{
    private readonly ConcurrentDictionary<Guid, Product> _products = new();

    public InMemoryProductRepository()
    {
        SeedData();
    }

    private void SeedData()
    {
        var products = new[]
        {
            new Product { Id = Guid.NewGuid(), Name = "Wireless Mouse", Description = "Ergonomic wireless mouse", Price = 29.99m, Category = "Electronics", CreatedAt = DateTime.UtcNow },
            new Product { Id = Guid.NewGuid(), Name = "Mechanical Keyboard", Description = "RGB mechanical keyboard", Price = 89.99m, Category = "Electronics", CreatedAt = DateTime.UtcNow },
            new Product { Id = Guid.NewGuid(), Name = "USB-C Hub", Description = "7-port USB-C hub", Price = 45.00m, Category = "Electronics", CreatedAt = DateTime.UtcNow },
            new Product { Id = Guid.NewGuid(), Name = "Standing Desk", Description = "Adjustable standing desk", Price = 499.99m, Category = "Furniture", CreatedAt = DateTime.UtcNow },
            new Product { Id = Guid.NewGuid(), Name = "Monitor Arm", Description = "Dual monitor arm", Price = 79.99m, Category = "Furniture", CreatedAt = DateTime.UtcNow },
            new Product { Id = Guid.NewGuid(), Name = "C# in Depth", Description = "Advanced C# programming book", Price = 39.99m, Category = "Books", CreatedAt = DateTime.UtcNow },
            new Product { Id = Guid.NewGuid(), Name = "Design Patterns", Description = "GoF Design Patterns book", Price = 44.99m, Category = "Books", CreatedAt = DateTime.UtcNow },
        };

        foreach (var product in products)
        {
            _products[product.Id] = product;
        }
    }

    public Task<PagedResult<Product>> GetAllAsync(ProductFilter filter)
    {
        var query = _products.Values.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(filter.Category))
            query = query.Where(p => p.Category == filter.Category);

        if (filter.MinPrice.HasValue)
            query = query.Where(p => p.Price >= filter.MinPrice.Value);

        if (filter.MaxPrice.HasValue)
            query = query.Where(p => p.Price <= filter.MaxPrice.Value);

        if (!string.IsNullOrWhiteSpace(filter.Search))
            query = query.Where(p => p.Name.Contains(filter.Search, StringComparison.OrdinalIgnoreCase));

        var totalCount = query.Count();

        var items = query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToList();

        return Task.FromResult(new PagedResult<Product>(items, totalCount));
    }

    public Task<Product?> GetByIdAsync(Guid id)
    {
        _products.TryGetValue(id, out var product);
        return Task.FromResult(product);
    }

    public Task<Product> CreateAsync(Product product)
    {
        product.Id = Guid.NewGuid();
        product.CreatedAt = DateTime.UtcNow;
        _products[product.Id] = product;
        return Task.FromResult(product);
    }

    public Task<Product?> UpdateAsync(Guid id, Product product)
    {
        if (!_products.TryGetValue(id, out var existing))
            return Task.FromResult<Product?>(null);

        existing.Name = product.Name;
        existing.Description = product.Description;
        existing.Price = product.Price;
        existing.Category = product.Category;
        existing.UpdatedAt = DateTime.UtcNow;

        _products[id] = existing;
        return Task.FromResult<Product?>(existing);
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        return Task.FromResult(_products.TryRemove(id, out _));
    }
}
