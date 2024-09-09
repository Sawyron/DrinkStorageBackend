namespace DrinkStorage.Application.Orders.Commands;
public record OrderItemCreateCommand(Guid ProductId, int Quantity);
