using Microsoft.Extensions.DependencyInjection;
using VShop.CartApi.Application.Mappings;
using VShop.CartApi.Application.UseCases;

namespace VShop.CartApi.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => cfg.AddMaps(typeof(MappingProfile).Assembly));

        services.AddScoped<GetCartUseCase>();
        services.AddScoped<UpdateCartUseCase>();
        services.AddScoped<DeleteCartItemUseCase>();
        services.AddScoped<ApplyCouponUseCase>();
        services.AddScoped<RemoveCouponUseCase>();
        services.AddScoped<CheckoutUseCase>();

        return services;
    }
}
