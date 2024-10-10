using System.Data;

namespace PetFamily.Species.Application;

public interface ISpeciesUnitOfWork
{
    public Task<IDbTransaction> BeginTransaction(CancellationToken token = default);
    public Task SaveChanges(CancellationToken token = default);
}