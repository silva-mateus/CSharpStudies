using Microsoft.Extensions.DependencyInjection;
using VShop.DiscountApi.Application.Mappings;
using VShop.DiscountApi.Application.UseCases.Coupons;

namespace VShop.DiscountApi.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => cfg.AddMaps(typeof(MappingProfile).Assembly));

        services.AddScoped<GetAllCouponsUseCase>();
        services.AddScoped<GetCouponByIdUseCase>();
        services.AddScoped<GetCouponByCodeUseCase>();
        services.AddScoped<CreateCouponUseCase>();
        services.AddScoped<UpdateCouponUseCase>();
        services.AddScoped<DeleteCouponUseCase>();

        return services;
    }
}
