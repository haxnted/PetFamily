using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Species.Application;

namespace PetFamily.Species.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddSpeciesInfrastructure(this IServiceCollection collection,
        IConfiguration configuration)
    {
        collection.AddScoped<SpeciesWriteDbContext>();
        collection.AddScoped<ISpeciesReadDbContext, SpeciesReadDbContext>();
        collection.AddScoped<ISpeciesRepository, SpeciesRepository>();
        
        collection.AddScoped<ISpeciesUnitOfWork, SpeciesUnitOfWork>();
        return collection;
    }
}
