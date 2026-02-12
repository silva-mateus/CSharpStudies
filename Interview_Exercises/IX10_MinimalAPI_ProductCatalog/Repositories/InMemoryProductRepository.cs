using System.Collections.Concurrent;
using IX10_MinimalAPI_ProductCatalog.Models;

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
        // TODO: your code goes here
        // 1. Start with all products.
        // 2. Apply filters: Category (exact match), MinPrice, MaxPrice, Search (name contains, case-insensitive).
        // 3. Order by CreatedAt descending.
        // 4. Apply pagination (Skip/Take based on Page and PageSize).
        // 5. Return PagedResult with items and total count (before pagination).
        throw new NotImplementedException();
    }

    public Task<Product?> GetByIdAsync(Guid id)
    {
        // TODO: your code goes here
        throw new NotImplementedException();
    }

    public Task<Product> CreateAsync(Product product)
    {
        // TODO: your code goes here
        // Assign Id and CreatedAt if not set.
        throw new NotImplementedException();
    }

    public Task<Product?> UpdateAsync(Guid id, Product product)
    {
        // TODO: your code goes here
        // Return null if product doesn't exist.
        // Update the existing product's fields and set UpdatedAt.
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        // TODO: your code goes here
        throw new NotImplementedException();
    }
}
