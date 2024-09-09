namespace DrinkStorage.WebApi.Products.Responses;

public record ProductInfoResponse(List<ProductResponse> Products, int MaxPrice);
