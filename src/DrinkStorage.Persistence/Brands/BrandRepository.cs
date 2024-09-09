using Microsoft.EntityFrameworkCore;

namespace DrinkStorage.Persistence.Brands;

public class BrandRepository
{
    private readonly ApplicationDbContext _context;

    public BrandRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<List<Brand>> FindAllAsync(CancellationToken token) => _context.Set<Brand>()
        .AsNoTracking()
        .ToListAsync(token);
}
