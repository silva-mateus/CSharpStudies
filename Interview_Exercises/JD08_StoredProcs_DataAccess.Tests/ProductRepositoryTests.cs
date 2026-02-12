using JD08_StoredProcs_DataAccess;
using Xunit;
using FluentAssertions;

namespace JD08_StoredProcs_DataAccess.Tests;

/// <summary>
/// TODO: Write integration tests that verify BOTH AdoNetProductRepository
/// and DapperProductRepository produce identical results.
///
/// SETUP:
/// - Use a unique test database per test class: "JD08_Test_{Guid}"
/// - Create the database and table in the constructor
/// - Drop the database in Dispose()
/// - Seed test data before each test or in the constructor
///
/// PARAMETERIZATION:
/// Use [Theory] + [MemberData] to run each test with both repository types.
/// Example:
///   public static IEnumerable&lt;object[]&gt; Repositories()
///   {
///       var connStr = $"Server=(localdb)\\MSSQLLocalDB;Database=...;Trusted_Connection=True;";
///       yield return new object[] { new AdoNetProductRepository(connStr), "ADO.NET" };
///       yield return new object[] { new DapperProductRepository(connStr), "Dapper" };
///   }
///
/// TESTS:
///  1. GetByIdAsync returns correct product for valid Id.
///  2. GetByIdAsync returns null for non-existent Id.
///  3. GetByCategoryAsync returns only products in that category.
///  4. GetByCategoryAsync returns empty for unknown category.
///  5. CreateAsync inserts product and returns it with Id.
///  6. UpdateStockAsync adds quantity successfully.
///  7. UpdateStockAsync subtracts quantity successfully.
///  8. UpdateStockAsync returns false when stock would go negative.
///  9. GetInventoryReportAsync returns correct aggregations.
/// 10. Both repositories return identical results for the same data.
/// </summary>
public class ProductRepositoryTests : IDisposable
{
    // TODO: Set up test database, create both repositories, write tests

    public void Dispose()
    {
        // TODO: Drop test database
    }
}
