namespace IX10_MinimalAPI_ProductCatalog.Models;

public record CreateProductRequest(
    string Name,
    string? Description,
    decimal Price,
    string Category);

public record UpdateProductRequest(
    string Name,
    string? Description,
    decimal Price,
    string Category);

public record PagedResponse<T>(
    List<T> Items,
    int Page,
    int PageSize,
    int TotalCount,
    int TotalPages);

public record ProductFilter(
    string? Category = null,
    decimal? MinPrice = null,
    decimal? MaxPrice = null,
    string? Search = null,
    int Page = 1,
    int PageSize = 10);
