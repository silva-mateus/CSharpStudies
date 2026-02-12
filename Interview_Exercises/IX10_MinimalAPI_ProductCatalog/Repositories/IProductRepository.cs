using IX10_MinimalAPI_ProductCatalog.Models;

namespace IX10_MinimalAPI_ProductCatalog.Repositories;

public record PagedResult<T>(List<T> Items, int TotalCount);

public interface IProductRepository
{
    Task<PagedResult<Product>> GetAllAsync(ProductFilter filter);
    Task<Product?> GetByIdAsync(Guid id);
    Task<Product> CreateAsync(Product product);
    Task<Product?> UpdateAsync(Guid id, Product product);
    Task<bool> DeleteAsync(Guid id);
}
