using VShop.ProductApi.Application.DTOs;
using VShop.ProductApi.Application.UseCases.Products;

namespace VShop.ProductApi.Api.Endpoints;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/products")
                       .RequireAuthorization();

        group.MapGet("/", async (GetAllProductsUseCase useCase) =>
            Results.Ok(await useCase.ExecuteAsync()));

        group.MapGet("/{id:int}", async (int id, GetProductByIdUseCase useCase) =>
        {
            var product = await useCase.ExecuteAsync(id);
            return product is null ? Results.NotFound("Product not found") : Results.Ok(product);
        }).WithName("GetProduct");

        group.MapPost("/", async (ProductDTO dto, CreateProductUseCase useCase) =>
        {
            var created = await useCase.ExecuteAsync(dto);
            return Results.CreatedAtRoute("GetProduct", new { id = created.Id }, created);
        }).RequireAuthorization(p => p.RequireRole("Admin"));

        group.MapPut("/", async (ProductDTO dto, UpdateProductUseCase useCase) =>
        {
            var updated = await useCase.ExecuteAsync(dto);
            return Results.Ok(updated);
        }).RequireAuthorization(p => p.RequireRole("Admin"));

        group.MapDelete("/{id:int}", async (int id, DeleteProductUseCase useCase) =>
        {
            await useCase.ExecuteAsync(id);
            return Results.NoContent();
        }).RequireAuthorization(p => p.RequireRole("Admin"));
    }
}
