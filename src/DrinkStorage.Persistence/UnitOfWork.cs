
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace DrinkStorage.Persistence;

internal class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IDbTransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
    {
        var transaction = _context.Database.BeginTransaction();
        return transaction.GetDbTransaction();
    }

    public async Task<int> SaveChangesAsync(CancellationToken token = default) =>
        await _context.SaveChangesAsync(token);
}
