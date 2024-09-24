using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Dto;
using PetFamily.Application.Extensions;
using PetFamily.Application.Features.Species.Queries.GetAllSpeciesWithPagination;
using PetFamily.Application.Models;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;

namespace PetFamily.Application.Features.Species.Queries.GetBreedsBySpecial;

public class GetBreedsBySpecialHandler(
    IReadDbContext readDbContext,
    IValidator<GetBreedsBySpecialQuery> validator,
    ILogger<GetSpeciesWithPaginationHandler> logger) : IQueryHandler<IEnumerable<BreedDto>, GetBreedsBySpecialQuery>
{
    public async Task<Result<IEnumerable<BreedDto>, ErrorList>> Execute(GetBreedsBySpecialQuery query,
        CancellationToken token = default)
    {
        var validationResult = await validator.ValidateAsync(query, token);
        if (validationResult.IsValid == false)
            return validationResult.ToList();

        var speciesQuery = readDbContext.Species
            .FirstOrDefault(s => s.Id == query.SpecialId);

        if (speciesQuery is null)
            return Errors.General.NotFound(query.SpecialId).ToErrorList();

        var breedsQuery = readDbContext.Breeds
            .Where(b => b.SpeciesId == query.SpecialId);

        breedsQuery = query.SortDirection?.ToLower() == "desc"
            ? breedsQuery.OrderByDescending(keySelector => keySelector.Name)
            : breedsQuery.OrderBy(keySelector => keySelector.Name);

        logger.Log(LogLevel.Information, "Get Breeds by Special with speciesId {speciesId}", query.SpecialId);

        return breedsQuery.ToList();
    }
}