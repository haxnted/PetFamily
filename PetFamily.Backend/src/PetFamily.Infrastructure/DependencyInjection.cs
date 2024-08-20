using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Volunteers;
using PetFamily.Infrastructure.Repositories;

namespace PetFamily.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddAInfrastructure(this IServiceCollection collection)
    {
        collection.AddScoped<ApplicationDbContext>();
        collection.AddScoped<IVolunteersRepository, VolunteersRepository>();
        return collection;
    }
}