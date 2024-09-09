using Microsoft.EntityFrameworkCore;

namespace DrinkStorage.Persistence.Products;

public class ProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<List<Product>> FindAllAsync(CancellationToken token) => _context.Set<Product>()
        .AsNoTracking()
        .ToListAsync(token);

    public Task<List<Product>> FindByIdsAsync(ICollection<Guid> ids, CancellationToken token) => _context.Set<Product>()
        .AsNoTracking()
        .Where(p => ids.Contains(p.Id))
        .ToListAsync(token);

    public Task<List<Product>> FindByBrandAndPrice(Guid? brandId, int maxPrice, CancellationToken token)
    {
        IQueryable<Product> query = _context.Set<Product>().AsNoTracking();
        if (brandId is not null)
        {
            query = query.Where(p => p.BrandId == brandId);
        }
        return query.Where(p => p.Price <= maxPrice).ToListAsync(token);
    }

    public Task ReduceQuantityAsync(Guid productId, int quantity, CancellationToken token) => _context.Set<Product>()
            .Where(p => p.Id == productId)
            .ExecuteUpdateAsync(p => p.SetProperty(p => p.Quantity, p => p.Quantity - quantity), token);
}
