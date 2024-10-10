using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dto;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.Species.Application.Queries.GetAllSpeciesWithPagination;

namespace PetFamily.Species.Application.Queries.GetBreedsBySpecial;

public class GetBreedsBySpecialHandler(
    ISpeciesReadDbContext speciesReadDbContext,
    IValidator<GetBreedsBySpecialQuery> validator,
    ILogger<GetSpeciesWithPaginationHandler> logger) : IQueryHandler<IEnumerable<BreedDto>, GetBreedsBySpecialQuery>
{
    public async Task<Result<IEnumerable<BreedDto>, ErrorList>> Execute(GetBreedsBySpecialQuery query,
        CancellationToken token = default)
    {
        var validationResult = await validator.ValidateAsync(query, token);
        if (validationResult.IsValid == false)
            return validationResult.ToList();

        var speciesQuery = speciesReadDbContext.Species
            .FirstOrDefault(s => s.Id == query.SpecialId);

        if (speciesQuery is null)
            return Errors.General.NotFound(query.SpecialId).ToErrorList();

        var breedsQuery = speciesReadDbContext.Breeds
            .Where(b => b.SpeciesId == query.SpecialId);

        breedsQuery = query.SortDirection?.ToLower() == "desc"
            ? breedsQuery.OrderByDescending(keySelector => keySelector.Name)
            : breedsQuery.OrderBy(keySelector => keySelector.Name);

        logger.Log(LogLevel.Information, "Get Breeds by Special with speciesId {speciesId}", query.SpecialId);

        return breedsQuery.ToList();
    }
}