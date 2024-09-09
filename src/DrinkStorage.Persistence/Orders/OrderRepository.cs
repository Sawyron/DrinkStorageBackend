namespace DrinkStorage.Persistence.Orders;

public class OrderRepository
{
    private readonly ApplicationDbContext _context;

    public OrderRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(Order order, CancellationToken token) => await _context.AddAsync(order, token);
}
