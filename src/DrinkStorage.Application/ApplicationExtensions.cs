using DrinkStorage.Application.Brands;
using DrinkStorage.Application.Change;
using DrinkStorage.Application.Coins;
using DrinkStorage.Application.Orders;
using DrinkStorage.Application.Products;
using Microsoft.Extensions.DependencyInjection;

namespace DrinkStorage.Application;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ProductService>();
        services.AddScoped<BrandService>();
        services.AddScoped<CoinService>();
        services.AddScoped<OrderService>();
        services.AddScoped<ChangeService>();
        return services;
    }
}
