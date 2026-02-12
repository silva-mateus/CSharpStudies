using FluentValidation;
using IX10_MinimalAPI_ProductCatalog.Models;
using IX10_MinimalAPI_ProductCatalog.Repositories;
using IX10_MinimalAPI_ProductCatalog.Validators;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddSingleton<IProductRepository, InMemoryProductRepository>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateProductRequestValidator>();

var app = builder.Build();

// TODO: Add global error handling middleware
// app.Use(async (context, next) => { ... });

// TODO: Define the following endpoints:

// GET /products - List products with filtering and pagination
// Query params: category, minPrice, maxPrice, search, page, pageSize
app.MapGet("/products", async (
    IProductRepository repo,
    string? category,
    decimal? minPrice,
    decimal? maxPrice,
    string? search,
    int? page,
    int? pageSize) =>
{
    // TODO: your code goes here
    // 1. Create ProductFilter from query params (with defaults and pageSize capped at 50).
    // 2. Call repo.GetAllAsync(filter).
    // 3. Return PagedResponse with calculated TotalPages.
    return Results.Ok("Not implemented");
});

// GET /products/{id} - Get single product
app.MapGet("/products/{id:guid}", async (Guid id, IProductRepository repo) =>
{
    // TODO: your code goes here
    // Return the product or Results.NotFound()
    return Results.Ok("Not implemented");
});

// POST /products - Create a product
app.MapPost("/products", async (
    CreateProductRequest request,
    IValidator<CreateProductRequest> validator,
    IProductRepository repo) =>
{
    // TODO: your code goes here
    // 1. Validate request. If invalid, return Results.ValidationProblem(errors).
    // 2. Map DTO to Product entity.
    // 3. Call repo.CreateAsync.
    // 4. Return Results.Created($"/products/{product.Id}", product).
    return Results.Ok("Not implemented");
});

// PUT /products/{id} - Update a product
app.MapPut("/products/{id:guid}", async (
    Guid id,
    UpdateProductRequest request,
    IValidator<UpdateProductRequest> validator,
    IProductRepository repo) =>
{
    // TODO: your code goes here
    // 1. Validate request.
    // 2. Map DTO to Product entity.
    // 3. Call repo.UpdateAsync. If null, return NotFound.
    // 4. Return Ok with updated product.
    return Results.Ok("Not implemented");
});

// DELETE /products/{id} - Delete a product
app.MapDelete("/products/{id:guid}", async (Guid id, IProductRepository repo) =>
{
    // TODO: your code goes here
    // Delete or return NotFound
    return Results.Ok("Not implemented");
});

app.Run();

// Required for WebApplicationFactory in integration tests
public partial class Program { }
