namespace DrinkStorage.Application.Orders.Commands;

public record CreateOrderCommand(
    List<OrderItemCreateCommand> OrderItems,
    List<CreateCoinCommand> Coins);
