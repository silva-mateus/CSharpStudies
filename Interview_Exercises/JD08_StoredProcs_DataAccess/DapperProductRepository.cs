using Dapper;
using Microsoft.Data.SqlClient;

namespace JD08_StoredProcs_DataAccess;

public class DapperProductRepository : IProductRepository
{
    private readonly string _connectionString;

    public DapperProductRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    // TODO: Implement all methods using Dapper.
    // Use QueryAsync<T>, QuerySingleOrDefaultAsync<T>, ExecuteAsync.
    // Use anonymous objects for parameters: new { Id = id }.
    // Compare code verbosity with the ADO.NET version.

    public Task<Product?> GetByIdAsync(int id) => throw new NotImplementedException();
    public Task<IReadOnlyList<Product>> GetByCategoryAsync(string category) => throw new NotImplementedException();
    public Task<Product> CreateAsync(string name, string category, decimal price, int stockQuantity) => throw new NotImplementedException();
    public Task<bool> UpdateStockAsync(int productId, int quantityChange) => throw new NotImplementedException();
    public Task<IReadOnlyList<InventoryReport>> GetInventoryReportAsync() => throw new NotImplementedException();
}
