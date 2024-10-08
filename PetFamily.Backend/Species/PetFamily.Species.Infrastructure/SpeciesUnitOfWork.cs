using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using PetFamily.Species.Application;

namespace PetFamily.Species.Infrastructure;

public class SpeciesUnitOfWork(SpeciesWriteDbContext context) : ISpeciesUnitOfWork
{
    public async Task<IDbTransaction> BeginTransaction(CancellationToken token = default)
    {
        var transaction = await context.Database.BeginTransactionAsync(token);
        return transaction.GetDbTransaction();
    }

    public async Task SaveChanges(CancellationToken token = default) =>
        await context.SaveChangesAsync(token);
}