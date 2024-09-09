using DrinkStorage.Persistence.Brands;
using DrinkStorage.Persistence.Coins;
using DrinkStorage.Persistence.Orders;
using DrinkStorage.Persistence.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DrinkStorage.Persistence;
public static class PersistenceExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("PostgreSQL")));
        services.AddScoped<ProductRepository>();
        services.AddScoped<BrandRepository>();
        services.AddScoped<CoinRepository>();
        services.AddScoped<OrderRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }

    public static bool EnsureDbCreated(this IServiceProvider serviceProvider)
    {
        using IServiceScope scope = serviceProvider.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        return context.Database.EnsureCreated();
    }

    public static void PopulateDb(this IServiceProvider serviceProvider)
    {
        using IServiceScope scope = serviceProvider.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        List<Coin> coins =
        [
            new() { Value = 1, Count = 100 },
            new() { Value = 2, Count = 100 },
            new() { Value = 5, Count = 100 },
            new() { Value = 10, Count = 100 }
        ];
        List<Brand> brands =
        [
            new () { Name = "The Coca-Cola Company" },
            new () { Name = "PepsiCo" },
            new () { Name = "Добрый" }
        ];
        List<Product> products =
        [
            new () {
                Name = "Coca Cola",
                Brand = brands[0],
                ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcStVaxw3MaNa5azFn13O2u5k-CZEw3dIJf_zA&s",
                Price = 100,
                Quantity = 10
            },
            new () {
                Name = "Pepsi Cola",
                Brand = brands[1],
                ImageUrl = "https://nnov.luding.ru/upload/resize_cache/iblock/d65/800_900_0/cijd7ldy20oeindmqgc6o4gccelbb2hm.png",
                Price = 150,
                Quantity = 10
            },
            new () {
                Name = "Добрый кола zero",
                Brand = brands[2],
                ImageUrl = "https://dobry.ru/local/templates/dobry/bundles/gazirovka/images/0.3/cola-zero.png",
                Price = 50,
                Quantity = 10
            },
            new () {
                Name = "Добрый кола ваниль",
                Brand = brands[2],
                ImageUrl = "https://dobry.ru/local/templates/dobry/bundles/gazirovka/images/0.33/vanil.png",
                Price = 50,
                Quantity = 10
            },
            new () {
                Name = "Добрый кола каремель",
                Brand = brands[2],
                ImageUrl = "https://dobry.ru/local/templates/dobry/bundles/gazirovka/images/0.5/caramel.png",
                Price = 50,
                Quantity = 10
            },
            new () {
                Name = "Добрый кола малина",
                Brand = brands[2],
                ImageUrl = "https://dobry.ru/local/templates/dobry/bundles/gazirovka/images/0.5/raspberries.png",
                Price = 50,
                Quantity = 10
            },
            new () {
                Name = "Добрый кола лесные ягоды",
                Brand = brands[2],
                ImageUrl = "https://dobry.ru/local/templates/dobry/bundles/gazirovka/images/0.5/berries.png",
                Price = 50,
                Quantity = 10
            },
            new () {
                Name = "Добрый кола апельсин",
                Brand = brands[2],
                ImageUrl = "https://dobry.ru/local/templates/dobry/bundles/gazirovka/images/0.33/orange.png",
                Price = 50,
                Quantity = 10
            },
            new () {
                Name = "Добрый кола лайм",
                Brand = brands[2],
                ImageUrl = "https://dobry.ru/local/templates/dobry/bundles/gazirovka/images/0.33/lime.png",
                Price = 50,
                Quantity = 0
            },
        ];
        context.AddRange(brands);
        context.AddRange(products);
        context.AddRange(coins);
        context.SaveChanges();
    }
}
