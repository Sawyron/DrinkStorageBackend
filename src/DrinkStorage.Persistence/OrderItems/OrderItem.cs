﻿using DrinkStorage.Persistence.Orders;
using DrinkStorage.Persistence.Products;

namespace DrinkStorage.Persistence.OrderItems;

public class OrderItem
{
    public Guid Id { get; set; }
    public int Price { get; set; }
    public int Quantity { get; set; }
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = default!;
    public Guid OrderId { get; set; }
    public Order Order { get; set; } = default!;
}
