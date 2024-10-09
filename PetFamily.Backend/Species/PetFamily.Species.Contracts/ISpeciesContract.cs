using CSharpFunctionalExtensions;
using PetFamily.Core.Dto;
using PetFamily.SharedKernel;

namespace PetFamily.Species.Contracts;

public interface ISpeciesContract
{
    public Task<Result<SpeciesDto, Error>> GetSpeciesById(
        Guid speciesId, CancellationToken cancellationToken);
}
