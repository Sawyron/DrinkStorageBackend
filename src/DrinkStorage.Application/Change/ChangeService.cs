using DrinkStorage.Application.Change.Responses;
using DrinkStorage.Application.Orders.Commands;
using DrinkStorage.Persistence.Coins;

namespace DrinkStorage.Application.Change;

public class ChangeService
{
    private readonly CoinRepository _coinRepository;

    public ChangeService(CoinRepository coinRepository)
    {
        _coinRepository = coinRepository;
    }

    public async Task<List<CoinResponse>?> GetChageAsync(int price, List<CoinCommand> incomingCoins, CancellationToken token)
    {
        var response = new List<CoinResponse>();
        List<Coin> coins = [.. (await _coinRepository.FindAllAsync(token)).OrderByDescending(c => c.Value)];
        var coinMap = coins.ToDictionary(c => c.Id, c => c);
        int given = incomingCoins.Sum(c => c.Quantity * coinMap[c.Id].Value);
        int change = given - price;
        foreach (Coin coin in coins)
        {
            int currnetAmount = Math.Min(coin.Count, change / coin.Value);
            if (currnetAmount > 0)
            {
                change -= currnetAmount * coin.Value;
                response.Add(new CoinResponse(coin.Id, coin.Value, currnetAmount));
            }
            if (change == 0)
            {
                break;
            }
        }
        return change == 0 ? response : null;
    }
}
