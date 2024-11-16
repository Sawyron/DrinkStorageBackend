using DrinkStorage.Application.Orders;
using DrinkStorage.Application.Orders.Commands;
using DrinkStorage.WebApi.Orders.Requests;
using Microsoft.AspNetCore.Mvc;

namespace DrinkStorage.WebApi.Orders;

[Route("api/v1/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly OrderService _service;

    public OrderController(OrderService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderRequest request, CancellationToken token)
    {
        var command = new CreateOrderCommand(
            request.OrderItems.Select(i => new OrderItemCreateCommand(i.ProductId, i.Quantity)).ToList(),
            request.Coins.Select(c => new CreateCoinCommand(c.Id, c.Quantity)).ToList());
        var response = await _service.CreateOrder(command, token);
        return response is not null ? Ok(response) : BadRequest("Unable to give change");
    }
}
