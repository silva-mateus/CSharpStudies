# IX10 - Minimal API: Product Catalog

## Difficulty: Hard
## Estimated Time: 90-120 minutes
## Type: Create from scratch (with integration tests)

## Overview

Build a RESTful Product Catalog API using .NET Minimal APIs. This exercise tests your ability to design a clean API with proper validation, error handling, pagination, and integration testing using `WebApplicationFactory`.

## Requirements

### Data Models

#### `Product` (entity)
| Property | Type | Description |
|----------|------|-------------|
| Id | Guid | Unique identifier (generated on creation) |
| Name | string | Product name (required, 1-200 chars) |
| Description | string? | Optional description (max 1000 chars) |
| Price | decimal | Price (required, > 0) |
| Category | string | Category name (required, non-empty) |
| CreatedAt | DateTime | UTC timestamp of creation |
| UpdatedAt | DateTime? | UTC timestamp of last update |

#### `CreateProductRequest` (DTO)
| Property | Type | Validation |
|----------|------|------------|
| Name | string | Required, 1-200 chars |
| Description | string? | Optional, max 1000 chars |
| Price | decimal | Required, > 0 |
| Category | string | Required, non-empty |

#### `UpdateProductRequest` (DTO)
| Property | Type | Validation |
|----------|------|------------|
| Name | string | Required, 1-200 chars |
| Description | string? | Optional, max 1000 chars |
| Price | decimal | Required, > 0 |
| Category | string | Required, non-empty |

#### `PagedResponse<T>`
| Property | Type | Description |
|----------|------|-------------|
| Items | List\<T\> | Page of results |
| Page | int | Current page (1-based) |
| PageSize | int | Items per page |
| TotalCount | int | Total items matching filters |
| TotalPages | int | Calculated total pages |

### API Endpoints

| Method | Route | Description | Success | Errors |
|--------|-------|-------------|---------|--------|
| GET | `/products` | List products (filtered, paged) | 200 + PagedResponse | - |
| GET | `/products/{id}` | Get product by ID | 200 + Product | 404 |
| POST | `/products` | Create a product | 201 + Product + Location header | 400 (validation) |
| PUT | `/products/{id}` | Update a product | 200 + Product | 400, 404 |
| DELETE | `/products/{id}` | Delete a product | 204 | 404 |

### GET /products Query Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| category | string? | null | Filter by exact category match |
| minPrice | decimal? | null | Filter by minimum price (inclusive) |
| maxPrice | decimal? | null | Filter by maximum price (inclusive) |
| search | string? | null | Search by name (contains, case-insensitive) |
| page | int | 1 | Page number (1-based) |
| pageSize | int | 10 | Items per page (max 50) |

### Repository

#### `IProductRepository`
- `Task<PagedResult<Product>> GetAllAsync(ProductFilter filter)` - Filtered + paged
- `Task<Product?> GetByIdAsync(Guid id)` - Single product
- `Task<Product> CreateAsync(Product product)` - Create and return
- `Task<Product?> UpdateAsync(Guid id, Product product)` - Update if exists
- `Task<bool> DeleteAsync(Guid id)` - Delete if exists

#### `InMemoryProductRepository`
- Stores products in a `ConcurrentDictionary<Guid, Product>`.
- Seed with 5-10 sample products on startup.

### Validation

Use `FluentValidation` (or manual validation if you prefer) to validate:
- `CreateProductRequest` and `UpdateProductRequest` inputs.
- Return `Results.ValidationProblem(errors)` for invalid input.

### Error Handling

- Add global error handling middleware that catches unhandled exceptions and returns `Results.Problem()` with a 500 status.
- Return `Results.NotFound()` for missing products.
- Return `Results.ValidationProblem()` for validation failures.

## Integration Tests to Write

Using `WebApplicationFactory<Program>` and `HttpClient`:

1. **GET /products** - Returns paged list with default pagination.
2. **GET /products?category=Electronics** - Filters by category.
3. **GET /products?minPrice=10&maxPrice=50** - Filters by price range.
4. **GET /products?search=widget** - Searches by name.
5. **GET /products?page=2&pageSize=2** - Pagination works correctly.
6. **GET /products/{id}** - Returns existing product.
7. **GET /products/{nonexistent-id}** - Returns 404.
8. **POST /products** - Creates product, returns 201 with Location header.
9. **POST /products** with invalid data - Returns 400 with validation errors.
10. **PUT /products/{id}** - Updates existing product.
11. **PUT /products/{nonexistent-id}** - Returns 404.
12. **DELETE /products/{id}** - Returns 204, product is gone.
13. **DELETE /products/{nonexistent-id}** - Returns 404.

## Constraints

- Use .NET Minimal APIs (NOT controllers).
- Use `ConcurrentDictionary` for the in-memory store (no EF Core / database).
- Register `IProductRepository` as a singleton in DI.
- The `Program.cs` must expose a partial class for `WebApplicationFactory` to work:
  ```csharp
  public partial class Program { }
  ```
- Use xUnit for tests.

## Topics Covered

- .NET Minimal APIs
- RESTful API design
- Request/Response DTOs
- Input validation (FluentValidation or manual)
- Pagination
- Dependency Injection in ASP.NET Core
- Integration testing with WebApplicationFactory
- ConcurrentDictionary for thread-safe storage
- Proper HTTP status codes and error responses
