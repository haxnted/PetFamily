using Microsoft.EntityFrameworkCore;
using PetFamily.Infrastructure;
using PetFamily.Infrastructure.DbContexts;

namespace PetFamily.API.Extensions;

public static class AppExtensions
{
    public static async Task ApplyMigrations(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<WriteDbContext>();
        await dbContext.Database.MigrateAsync();
    }
}