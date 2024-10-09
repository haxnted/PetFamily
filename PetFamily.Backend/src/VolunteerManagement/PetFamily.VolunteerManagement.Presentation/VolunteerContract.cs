using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Core.Dto;
using PetFamily.SharedKernel;
using PetFamily.VolunteerManagement.Application;
using PetFamily.VolunteerManagement.Contracts;

namespace PetFamily.VolunteerManagement.Presentation;

public class VolunteerContract(IVolunteersReadDbContext context) : IVolunteerContract
{
    public async Task<Result<PetDto, Error>> IsPetsUsedSpecies(Guid speciesId, CancellationToken cancellationToken)
    {
        var result = await context.Pets.FirstOrDefaultAsync(p => p.SpeciesId == speciesId, cancellationToken);
        if (result is not null)
            return result;
        return Errors.General.NotFound(speciesId);
    }

    public async Task<Result<PetDto, Error>> IsPetsUsedBreed(
        Guid breedId, CancellationToken cancellationToken)
    {
        var result = await context.Pets.FirstOrDefaultAsync(p => p.BreedId == breedId, cancellationToken);
        if (result is not null)
            return result;
        return Errors.General.NotFound(breedId);
    }
}