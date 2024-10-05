using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.VolunteerManagement.ValueObjects;

namespace PetFamily.Application.Features.Species;

public interface ISpeciesRepository
{
    public Task Add(Domain.Species.Species species,
        CancellationToken cancellationToken = default);

    public Task<Result<Domain.Species.Species, Error>> GetSpeciesById(
        SpeciesId id,
        CancellationToken cancellationToken = default);

    public Task<Result<Domain.Species.Species, Error>> GetSpeciesByName(
        TypeAnimal animal, 
        CancellationToken cancellationToken);

    public Task Save(Domain.Species.Species species, CancellationToken cancellationToken = default);

    public void Delete(Domain.Species.Species species, CancellationToken cancellationToken = default);
}