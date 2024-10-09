using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteerManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddVolunteerApplication(this IServiceCollection collection)
    {
        return collection.AddHandlers()
            .AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
    }

    private static IServiceCollection AddHandlers(this IServiceCollection collection)
    {
        collection.Scan(scan => scan.FromAssemblies(typeof(DependencyInjection).Assembly)
            .AddClasses(classes => classes
                .AssignableToAny(typeof(ICommandHandler<,>), typeof(ICommandHandler<>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());

        collection.Scan(scan => scan.FromAssemblies(typeof(DependencyInjection).Assembly)
            .AddClasses(classes => classes
                .AssignableTo(typeof(IQueryHandler<,>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());

        return collection;
    }
}