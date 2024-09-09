using DrinkStorage.Application.Coins;
using DrinkStorage.Persistence.Coins;
using DrinkStorage.WebApi.Coins.Responses;
using Microsoft.AspNetCore.Mvc;

namespace DrinkStorage.WebApi.Coins;

[Route("api/v1/[controller]")]
[ApiController]
public class CoinController : ControllerBase
{
    private readonly CoinService _service;

    public CoinController(CoinService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken token)
    {
        var coins = await _service.FindAllAsync(token);
        return Ok(coins.Select(MapCoin));
    }

    private static CoinResponse MapCoin(Coin coin) =>
        new(coin.Id, coin.Value);
}
