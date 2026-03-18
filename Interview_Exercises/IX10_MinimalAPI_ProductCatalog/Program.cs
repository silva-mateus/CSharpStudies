using FluentValidation;
using IX10_MinimalAPI_ProductCatalog.Models;
using IX10_MinimalAPI_ProductCatalog.Repositories;
using IX10_MinimalAPI_ProductCatalog.Validators;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddSingleton<IProductRepository, InMemoryProductRepository>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateProductRequestValidator>();

var app = builder.Build();

app.Use(async (context, next) =>
{
    try
    {
        await next(context);
    }
    catch (Exception)
    {
        if (!context.Response.HasStarted)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new { title = "An error occurred", status = 500 });
        }
    }
}

);

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
    var actualPageSize = Math.Min(pageSize ?? 10, 50);
    var filter = new ProductFilter(
        Category: category,
        MinPrice: minPrice,
        MaxPrice: maxPrice,
        Search: search,
        Page: page ?? 1,
        PageSize: actualPageSize);

    var result = await repo.GetAllAsync(filter);
    var totalPages = (int)Math.Ceiling((double)result.TotalCount / actualPageSize);

    return Results.Ok(new PagedResponse<Product>(
        result.Items,
        filter.Page,
        actualPageSize,
        result.TotalCount,
        totalPages));
});

// GET /products/{id} - Get single product
app.MapGet("/products/{id:guid}", async (Guid id, IProductRepository repo) =>
{
    var product = await repo.GetByIdAsync(id);
    return product is not null ? Results.Ok(product) : Results.NotFound();
});

// POST /products - Create a product
app.MapPost("/products", async (
    CreateProductRequest request,
    IValidator<CreateProductRequest> validator,
    IProductRepository repo) =>
{
    var validationResult = await validator.ValidateAsync(request);
    if (!validationResult.IsValid)
    {
        var errors = validationResult.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
        return Results.ValidationProblem(errors);
    }

    var product = new Product
    {
        Name = request.Name,
        Description = request.Description,
        Price = request.Price,
        Category = request.Category
    };

    var created = await repo.CreateAsync(product);
    return Results.Created($"/products/{created.Id}", created);
});

// PUT /products/{id} - Update a product
app.MapPut("/products/{id:guid}", async (
    Guid id,
    UpdateProductRequest request,
    IValidator<UpdateProductRequest> validator,
    IProductRepository repo) =>
{
    var validationResult = await validator.ValidateAsync(request);
    if (!validationResult.IsValid)
    {
        var errors = validationResult.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
        return Results.ValidationProblem(errors);
    }

    var product = new Product
    {
        Name = request.Name,
        Description = request.Description,
        Price = request.Price,
        Category = request.Category
    };

    var updated = await repo.UpdateAsync(id, product);
    return updated is not null ? Results.Ok(updated) : Results.NotFound();
});

// DELETE /products/{id} - Delete a product
app.MapDelete("/products/{id:guid}", async (Guid id, IProductRepository repo) =>
{
    var deleted = await repo.DeleteAsync(id);
    return deleted ? Results.NoContent() : Results.NotFound();
});

app.Run();


// Required for WebApplicationFactory in integration tests
public partial class Program { }