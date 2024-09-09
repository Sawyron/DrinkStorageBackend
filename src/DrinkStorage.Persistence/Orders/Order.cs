using DrinkStorage.Persistence.OrderItems;

namespace DrinkStorage.Persistence.Orders;

public class Order
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<OrderItem> Items { get; set; } = default!;
}
