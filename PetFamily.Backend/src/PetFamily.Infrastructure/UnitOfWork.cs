using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using PetFamily.Application.Database;

namespace PetFamily.Infrastructure;

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    public async Task<IDbTransaction> BeginTransaction(CancellationToken token = default)
    {
        var transaction = await context.Database.BeginTransactionAsync(token);
        return transaction.GetDbTransaction();
    }

    public async Task SaveChanges(CancellationToken token = default) =>
        await context.SaveChangesAsync(token);
}