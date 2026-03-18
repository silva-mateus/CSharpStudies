using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VShop.DiscountApi.Domain.Interfaces;
using VShop.DiscountApi.Infrastructure.Persistence;
using VShop.DiscountApi.Infrastructure.Persistence.Repositories;

namespace VShop.DiscountApi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (!string.IsNullOrEmpty(connectionString))
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
        }

        services.AddScoped<ICouponRepository, CouponRepository>();

        return services;
    }
}
