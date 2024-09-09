namespace DrinkStorage.WebApi.Orders.Requests;

public record OrderItemRequest(Guid ProductId, int Quantity);
