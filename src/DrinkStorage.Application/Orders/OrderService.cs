using DrinkStorage.Application.Change;
using DrinkStorage.Application.Change.Responses;
using DrinkStorage.Application.Orders.Commands;
using DrinkStorage.Application.Orders.Exceptions;
using DrinkStorage.Application.Orders.Responses;
using DrinkStorage.Persistence;
using DrinkStorage.Persistence.Coins;
using DrinkStorage.Persistence.OrderItems;
using DrinkStorage.Persistence.Orders;
using DrinkStorage.Persistence.Products;
using System.Data;

namespace DrinkStorage.Application.Orders;

public class OrderService
{
    private readonly OrderRepository _orderRepository;
    private readonly ProductRepository _productRepository;
    private readonly CoinRepository _coinRepository;
    private readonly ChangeService _changeService;
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(
        OrderRepository orderRepository,
        ProductRepository productRepository,
        ChangeService changeService,
        IUnitOfWork unitOfWork,
        CoinRepository coinRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _changeService = changeService;
        _unitOfWork = unitOfWork;
        _coinRepository = coinRepository;
    }

    public async Task<CreateOrderResponse?> CreateOrder(CreateOrderCommand command, CancellationToken token)
    {
        using IDbTransaction transaction = _unitOfWork.BeginTransaction();
        try
        {
            var products = (await _productRepository.FindByIdsAsync(
            command.OrderItems.Select(i => i.ProductId).ToList(),
            token))
            .ToDictionary(p => p.Id, p => p);
            foreach (var item in command.OrderItems)
            {
                if (!products.ContainsKey(item.ProductId))
                {
                    throw new ProductsNotFoundException([item.ProductId]);
                }
                if (products[item.ProductId].Quantity < item.Quantity)
                {
                    throw new ProductQuantityIsUnsufficientExcettion(item.ProductId, item.Quantity);
                }
            }
            int price = command.OrderItems
                .Sum(i => i.Quantity * products[i.ProductId].Price);
            List<CoinResponse>? change = await _changeService.GetChageAsync(price, command.Coins, token);
            if (change is null)
            {
                return null;
            }
            Task productTask = Task.WhenAll(command.OrderItems
                .Select(i => _productRepository.ReduceQuantityAsync(i.ProductId, i.Quantity, token)));
            Task coinTask = Task.WhenAll(command.Coins
                .Select(c => _coinRepository.ReduceQuantitiesAsync(c.Id, c.Quantity, token)));
            await Task.WhenAll(
                [
                    _orderRepository.CreateAsync(MapOrderCommand(command, products), token),
                productTask,
                coinTask,
            ]);
            await _unitOfWork.SaveChangesAsync(token);
            transaction.Commit();
            return new CreateOrderResponse(change
                .Select(c => new ChangeCoinResponse(c.Id, c.Value, c.Quantity))
                .ToList());
        }
        catch (Exception)
        {
            transaction.Rollback();
            return null;
        }
    }

    private static Order MapOrderCommand(CreateOrderCommand command, IDictionary<Guid, Product> products) =>
        new()
        {
            CreatedAt = DateTime.UtcNow,
            Items = command.OrderItems.Select(i => new OrderItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Price = products[i.ProductId].Price
            })
            .ToList()
        };
}
