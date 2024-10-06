using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using PetFamily.VolunteerManagement.Application;

namespace PetFamily.VolunteerManagement.Infrastructure;

public class VolunteerUnitOfWork(VolunteersWriteDbContext context) : IVolunteerUnitOfWork
{
    public async Task<IDbTransaction> BeginTransaction(CancellationToken token = default)
    {
        var transaction = await context.Database.BeginTransactionAsync(token);
        return transaction.GetDbTransaction();
    }

    public async Task SaveChanges(CancellationToken token = default) =>
        await context.SaveChangesAsync(token);
}