using Microsoft.Data.SqlClient;

namespace JD08_StoredProcs_DataAccess;

public class AdoNetProductRepository : IProductRepository
{
    private readonly string _connectionString;

    public AdoNetProductRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    // TODO: Implement all methods using raw ADO.NET (SqlConnection, SqlCommand, SqlDataReader).
    // Use parameterized queries (@param) for all inputs.
    // Use async methods (ExecuteReaderAsync, ExecuteNonQueryAsync, etc.).
    // Properly dispose connections, commands, and readers (using statements).

    public Task<Product?> GetByIdAsync(int id) => throw new NotImplementedException();
    public Task<IReadOnlyList<Product>> GetByCategoryAsync(string category) => throw new NotImplementedException();
    public Task<Product> CreateAsync(string name, string category, decimal price, int stockQuantity) => throw new NotImplementedException();
    public Task<bool> UpdateStockAsync(int productId, int quantityChange) => throw new NotImplementedException();
    public Task<IReadOnlyList<InventoryReport>> GetInventoryReportAsync() => throw new NotImplementedException();
}
