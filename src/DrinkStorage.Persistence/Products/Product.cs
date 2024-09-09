using DrinkStorage.Persistence.Brands;

namespace DrinkStorage.Persistence.Products;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Price { get; set; }
    public int Quantity { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public Guid BrandId { get; set; }
    public Brand Brand { get; set; } = default!;
}
