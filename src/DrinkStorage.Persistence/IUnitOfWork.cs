
using System.Data;

namespace DrinkStorage.Persistence;
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken token = default);
    IDbTransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
}
