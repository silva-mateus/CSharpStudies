using VShop.ProductApi.Application.DTOs;
using VShop.ProductApi.Application.UseCases.Categories;

namespace VShop.ProductApi.Api.Endpoints;

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/categories")
                       .RequireAuthorization();

        group.MapGet("/", async (GetAllCategoriesUseCase useCase) =>
            Results.Ok(await useCase.ExecuteAsync()));

        group.MapGet("/products", async (GetCategoriesWithProductsUseCase useCase) =>
            Results.Ok(await useCase.ExecuteAsync()));

        group.MapGet("/{id:int}", async (int id, GetCategoryByIdUseCase useCase) =>
        {
            var category = await useCase.ExecuteAsync(id);
            return category is null ? Results.NotFound("Category not found") : Results.Ok(category);
        }).WithName("GetCategory");

        group.MapPost("/", async (CategoryDTO dto, CreateCategoryUseCase useCase) =>
        {
            var created = await useCase.ExecuteAsync(dto);
            return Results.CreatedAtRoute("GetCategory", new { id = created.Id }, created);
        }).RequireAuthorization(p => p.RequireRole("Admin"));

        group.MapPut("/{id:int}", async (int id, CategoryDTO dto, UpdateCategoryUseCase useCase) =>
        {
            if (id != dto.Id)
                return Results.BadRequest("Id mismatch");
            var updated = await useCase.ExecuteAsync(dto);
            return Results.Ok(updated);
        }).RequireAuthorization(p => p.RequireRole("Admin"));

        group.MapDelete("/{id:int}", async (int id, DeleteCategoryUseCase useCase) =>
        {
            await useCase.ExecuteAsync(id);
            return Results.NoContent();
        }).RequireAuthorization(p => p.RequireRole("Admin"));
    }
}
