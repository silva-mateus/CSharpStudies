using Microsoft.Extensions.DependencyInjection;
using VShop.ProductApi.Application.Mappings;
using VShop.ProductApi.Application.UseCases.Categories;
using VShop.ProductApi.Application.UseCases.Products;

namespace VShop.ProductApi.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => cfg.AddMaps(typeof(MappingProfile).Assembly));

        services.AddScoped<GetAllProductsUseCase>();
        services.AddScoped<GetProductByIdUseCase>();
        services.AddScoped<CreateProductUseCase>();
        services.AddScoped<UpdateProductUseCase>();
        services.AddScoped<DeleteProductUseCase>();

        services.AddScoped<GetAllCategoriesUseCase>();
        services.AddScoped<GetCategoriesWithProductsUseCase>();
        services.AddScoped<GetCategoryByIdUseCase>();
        services.AddScoped<CreateCategoryUseCase>();
        services.AddScoped<UpdateCategoryUseCase>();
        services.AddScoped<DeleteCategoryUseCase>();

        return services;
    }
}
