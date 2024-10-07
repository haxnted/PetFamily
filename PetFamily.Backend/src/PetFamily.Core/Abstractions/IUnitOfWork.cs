using System.Data;

namespace PetFamily.Core.Abstractions;

public interface IUnitOfWork
{
    public Task<IDbTransaction> BeginTransaction(CancellationToken token = default);
    public Task SaveChanges(CancellationToken token = default);
}
