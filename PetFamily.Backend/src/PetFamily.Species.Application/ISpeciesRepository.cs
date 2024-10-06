using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.EntityIds;
using PetFamily.VolunteerManagement.Domain.ValueObjects;

namespace PetFamily.Species.Application;

public interface ISpeciesRepository
{
    public Task Add(Domain.Species species, CancellationToken cancellationToken = default);

    public Task<Result<Domain.Species, Error>> GetSpeciesById(
        SpeciesId id,
        CancellationToken cancellationToken = default);

    public Task<Result<Domain.Species, Error>> GetSpeciesByName(
        TypeAnimal animal, 
        CancellationToken cancellationToken);

    public void Save(Domain.Species species, CancellationToken cancellationToken = default);

    public void Delete(Domain.Species species, CancellationToken cancellationToken = default);
}