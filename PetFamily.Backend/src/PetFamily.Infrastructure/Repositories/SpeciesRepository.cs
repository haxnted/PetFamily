using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Features.Species;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.Species;
using PetFamily.Domain.VolunteerManagement.ValueObjects;
using PetFamily.Infrastructure.DbContexts;

namespace PetFamily.Infrastructure.Repositories;

public class SpeciesRepository(
    WriteDbContext context) : ISpeciesRepository
{
    public async Task<Result<Guid, Error>> Add(Species species, CancellationToken cancellationToken = default)
    {
        await context.Species.AddAsync(species, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return species.Id.Id;
    }

    public async Task<Result<Species, Error>> GetSpeciesById(SpeciesId id,
        CancellationToken cancellationToken = default)
    {
        var species = await context.Species
            .Include(s => s.Breeds)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        if (species is null)
            return Errors.General.NotFound(id.Id);

        return species;
    }

    public async Task<Result<Species, Error>> GetSpeciesByName(TypeAnimal typeAnimal, CancellationToken cancellationToken)
    {
        var species = await context.Species
            .Include(s => s.Breeds)
            .FirstOrDefaultAsync(s => s.TypeAnimal == typeAnimal, cancellationToken);
        if (species is null)
            return Errors.General.NotFound();

        return species;
    }

    public async Task<Result<Guid, Error>> Save(Species species, CancellationToken cancellationToken = default)
    {
        context.Species.Attach(species);
        await context.SaveChangesAsync(cancellationToken);

        return species.Id.Id;
    }

    public async Task<Result<Guid, Error>> Delete(Species species, CancellationToken cancellationToken = default)
    {
        context.Species.Remove(species);
        await context.SaveChangesAsync(cancellationToken);

        return species.Id.Id;
    }
}