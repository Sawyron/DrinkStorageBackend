namespace DrinkStorage.WebApi.Products.Responses;

public record ProductResponse(
    Guid Id,
    string Name,
    int Price,
    int Quantity,
    string ImageUrl,
    string BrandId);
