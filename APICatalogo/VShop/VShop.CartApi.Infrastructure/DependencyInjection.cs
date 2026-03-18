using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VShop.CartApi.Application.Interfaces;
using VShop.CartApi.Domain.Interfaces;
using VShop.CartApi.Infrastructure.Messaging;
using VShop.CartApi.Infrastructure.Persistence;
using VShop.CartApi.Infrastructure.Persistence.Repositories;

namespace VShop.CartApi.Infrastructure;

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

        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<IMessageProducer, NullMessageProducer>();

        return services;
    }
}
