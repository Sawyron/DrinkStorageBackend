using DrinkStorage.Persistence.Coins;

namespace DrinkStorage.Application.Coins;

public class CoinService
{
    private readonly CoinRepository _repository;

    public CoinService(CoinRepository repository)
    {
        _repository = repository;
    }

    public Task<List<Coin>> FindAllAsync(CancellationToken token) =>
        _repository.FindAllAsync(token);
}
