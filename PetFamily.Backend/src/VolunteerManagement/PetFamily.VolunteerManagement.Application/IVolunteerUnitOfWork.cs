using System.Data;

namespace PetFamily.VolunteerManagement.Application;

public interface IVolunteerUnitOfWork
{
    public Task<IDbTransaction> BeginTransaction(CancellationToken token = default);
    public Task SaveChanges(CancellationToken token = default);
}