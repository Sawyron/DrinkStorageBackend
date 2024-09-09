using DrinkStorage.Persistence.Products;

namespace DrinkStorage.Application.Products.Responses;

public record ProductInfo(List<Product> Products, int MaxPrice);
