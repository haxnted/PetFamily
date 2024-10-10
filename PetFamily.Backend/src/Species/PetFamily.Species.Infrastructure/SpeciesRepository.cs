using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.EntityIds;
using PetFamily.Species.Application;
using PetFamily.Species.Domain.ValueObjects;
using PetFamily.Species.Infrastructure.DbContexts;

namespace PetFamily.Species.Infrastructure;

public class SpeciesRepository(
    SpeciesWriteDbContext context) : ISpeciesRepository
{
    public async Task Add(Domain.Species species, CancellationToken cancellationToken = default)
    {
        await context.Species.AddAsync(species, cancellationToken);
    }

    public async Task<Result<Domain.Species, Error>> GetSpeciesById(SpeciesId id,
        CancellationToken cancellationToken = default)
    {
        var species = await context.Species
            .Include(s => s.Breeds)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        if (species is null)
            return Errors.General.NotFound(id.Id);

        return species;
    }

    public async Task<Result<Domain.Species, Error>> GetSpeciesByName(TypeAnimal typeAnimal, CancellationToken cancellationToken)
    {
        var species = await context.Species
            .Include(s => s.Breeds)
            .FirstOrDefaultAsync(s => s.TypeAnimal == typeAnimal, cancellationToken);
        if (species is null)
            return Errors.General.NotFound();

        return species;
    }

    public void Save(Domain.Species species, CancellationToken cancellationToken = default)
    {
        context.Species.Attach(species);
    }

    public void Delete(Domain.Species species, CancellationToken cancellationToken = default)
    {
        context.Species.Remove(species);
    }
}