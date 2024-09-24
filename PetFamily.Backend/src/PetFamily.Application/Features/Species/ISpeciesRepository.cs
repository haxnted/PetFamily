using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.VolunteerManagement.ValueObjects;

namespace PetFamily.Application.Features.Species;

public interface ISpeciesRepository
{
    public Task<Result<Guid, Error>> Add(
        Domain.Species.Species species,
        CancellationToken cancellationToken = default);

    public Task<Result<Domain.Species.Species, Error>> GetSpeciesById(
        SpeciesId id,
        CancellationToken cancellationToken = default);

    public Task<Result<Domain.Species.Species, Error>> GetSpeciesByName(
        TypeAnimal animal, 
        CancellationToken cancellationToken);

    public Task<Result<Guid, Error>> Save(
        Domain.Species.Species species, CancellationToken cancellationToken = default);

    public Task<Result<Guid, Error>> Delete(
        Domain.Species.Species species, CancellationToken cancellationToken = default);
}