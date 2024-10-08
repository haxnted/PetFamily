using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.VolunteerManagement.Application;
using PetFamily.VolunteerManagement.Contracts;
using PetFamily.VolunteerManagement.Infrastructure;

namespace PetFamily.VolunteerManagement.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddVolunteerModule(this IServiceCollection collection, IConfiguration configuration)
    {
        return collection.AddScoped<IVolunteerContract, VolunteerContract>()
            .AddVolunteerApplication()
            .AddVolunteerInfrastructure(configuration);
    }
}
