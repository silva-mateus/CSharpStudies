# JD08 - Stored Procedures and Data Access Patterns

## Difficulty: Hard
## Estimated Time: 90-120 minutes
## Type: Create from scratch (with integration tests)

## Overview

Build a data access layer for a Product Inventory system using three different approaches: stored procedures, raw ADO.NET, and Dapper. This exercise tests your depth of SQL Server knowledge, understanding of different data access patterns, and ability to write integration tests that verify database behavior.

## Setup

Run `setup.sql` against SQL Server LocalDB to create the `JD08_InventoryDB` database with the schema and seed data.

## Requirements

### Part 1: Stored Procedures (write in `stored-procedures.sql`)

Write 5 stored procedures:

1. **`usp_GetProductById`** - Takes `@ProductId INT`, returns the product or empty set.
2. **`usp_GetProductsByCategory`** - Takes `@Category NVARCHAR(100)`, returns all matching products ordered by Name.
3. **`usp_CreateProduct`** - Takes `@Name`, `@Category`, `@Price`, `@StockQuantity`. Returns the new product with its generated Id.
4. **`usp_UpdateProductStock`** - Takes `@ProductId INT`, `@QuantityChange INT`. Updates stock quantity (can be positive or negative). Throws error if stock would go below 0. Uses a transaction.
5. **`usp_GetInventoryReport`** - No parameters. Returns a summary: Category, ProductCount, TotalStockValue (sum of Price * StockQuantity), AvgPrice. Uses a CTE or temp table.

### Part 2: ADO.NET Repository (implement in `AdoNetProductRepository.cs`)

Implement `IProductRepository` using raw `SqlConnection` and `SqlCommand`:
- Use parameterized queries (NOT string concatenation)
- Use `SqlDataReader` for reads
- Properly dispose all connections and commands
- Use `async` methods throughout
- Handle `SqlException` appropriately

### Part 3: Dapper Repository (implement in `DapperProductRepository.cs`)

Implement the same `IProductRepository` using Dapper:
- Use `QueryAsync<T>`, `QuerySingleOrDefaultAsync<T>`, `ExecuteAsync`
- Use parameterized queries with anonymous objects
- Show the difference in code verbosity vs ADO.NET

### Part 4: Integration Tests

Write tests that verify BOTH implementations produce identical results:
- Each test should run against BOTH `AdoNetProductRepository` and `DapperProductRepository`
- Use `[Theory]` with `[MemberData]` to parameterize the repository implementation
- Tests run against SQL Server LocalDB with a test-specific database

### IProductRepository Interface

```csharp
public interface IProductRepository
{
    Task<Product?> GetByIdAsync(int id);
    Task<IReadOnlyList<Product>> GetByCategoryAsync(string category);
    Task<Product> CreateAsync(string name, string category, decimal price, int stockQuantity);
    Task<bool> UpdateStockAsync(int productId, int quantityChange);
    Task<IReadOnlyList<InventoryReport>> GetInventoryReportAsync();
}
```

## Constraints

- Use SQL Server LocalDB for all database operations.
- Both repositories must pass the same test suite.
- Stored procedures must handle edge cases (stock going negative, non-existent products).
- All database access must be async.

## Topics Covered

- SQL Server stored procedures (CREATE PROCEDURE, parameters, transactions, error handling)
- ADO.NET (SqlConnection, SqlCommand, SqlDataReader, parameterized queries)
- Dapper ORM (thin mapper)
- Repository pattern with multiple implementations
- Integration testing against SQL Server
- Connection management and disposal
- Async database access
