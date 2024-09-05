using System.Data;

namespace PetFamily.Application.Database;

public interface IUnitOfWork
{
    public Task<IDbTransaction> BeginTransaction(CancellationToken token = default);
    public Task SaveChanges(CancellationToken token = default);
}