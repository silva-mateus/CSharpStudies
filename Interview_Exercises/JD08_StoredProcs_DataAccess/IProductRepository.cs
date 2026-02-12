namespace JD08_StoredProcs_DataAccess;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(int id);
    Task<IReadOnlyList<Product>> GetByCategoryAsync(string category);
    Task<Product> CreateAsync(string name, string category, decimal price, int stockQuantity);
    Task<bool> UpdateStockAsync(int productId, int quantityChange);
    Task<IReadOnlyList<InventoryReport>> GetInventoryReportAsync();
}
