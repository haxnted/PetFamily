using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Core.Dto;
using PetFamily.SharedKernel;
using PetFamily.Species.Application;
using PetFamily.Species.Contracts;

namespace PetFamily.Species.Presentation;

public class SpeciesContract(ISpeciesReadDbContext readDbContext) : ISpeciesContract
{
    public async Task<Result<SpeciesDto, Error>> GetSpeciesById(
        Guid speciesId, CancellationToken cancellationToken)
    {
        var result = await readDbContext.Species.Include(s => s.Breeds)
            .FirstOrDefaultAsync(s => s.Id == speciesId, cancellationToken);

        if (result is not null)
            return result;

        return Errors.General.NotFound(speciesId);
    }
}
