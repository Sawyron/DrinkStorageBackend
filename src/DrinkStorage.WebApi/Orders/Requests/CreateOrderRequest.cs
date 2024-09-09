namespace DrinkStorage.WebApi.Orders.Requests;

public record CreateOrderRequest(List<OrderItemRequest> OrderItems, List<CoinRequest> Coins);
