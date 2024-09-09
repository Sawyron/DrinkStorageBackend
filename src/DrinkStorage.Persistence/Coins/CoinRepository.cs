using Microsoft.EntityFrameworkCore;

namespace DrinkStorage.Persistence.Coins;

public class CoinRepository
{
    private readonly ApplicationDbContext _context;

    public CoinRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<List<Coin>> FindAllAsync(CancellationToken token) => _context.Set<Coin>()
        .AsNoTracking()
        .OrderBy(c => c.Value)
        .ToListAsync(token);

    public Task<int> GetTotalValueAsync(CancellationToken token) => _context.Set<Coin>()
        .SumAsync(c => c.Value, token);

    public Task ReduceQuantitiesAsync(Guid coinId, int quantity, CancellationToken token) => _context.Set<Coin>()
        .Where(c => c.Id == coinId)
        .ExecuteUpdateAsync(c => c.SetProperty(c => c.Count, c => c.Count - quantity), token);
}
