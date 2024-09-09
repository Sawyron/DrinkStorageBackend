namespace DrinkStorage.Application.Products.Csv;
public class ProductCsv
{
    public string Name { get; set; } = string.Empty;
    public int Price { get; set; }
    public int Quantity { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public Guid BrandId { get; set; }
}
