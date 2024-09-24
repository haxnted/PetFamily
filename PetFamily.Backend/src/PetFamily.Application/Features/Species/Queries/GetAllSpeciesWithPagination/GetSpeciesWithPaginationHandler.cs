using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Dto;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Species.Queries.GetAllSpeciesWithPagination;

public class GetSpeciesWithPaginationHandler(
    IReadDbContext readDbContext,
    IValidator<GetSpeciesWithPaginationQuery> validator,
    ILogger<GetSpeciesWithPaginationHandler> logger) : IQueryHandler<PagedList<SpeciesDto>,GetSpeciesWithPaginationQuery>
{
    public async Task<Result<PagedList<SpeciesDto>, ErrorList>> Execute(GetSpeciesWithPaginationQuery query, CancellationToken token = default)
    {
        var validationResult = await validator.ValidateAsync(query, token);
        if (validationResult.IsValid == false)
            return validationResult.ToList();
        
        var speciesQuery = readDbContext.Species;

        speciesQuery = speciesQuery.Include(s => s.Breeds);
        
        speciesQuery = query.SortDirection?.ToLower() == "desc"
            ? speciesQuery.OrderByDescending(keySelector => keySelector.TypeAnimal)
            : speciesQuery.OrderBy(keySelector => keySelector.TypeAnimal);

        var result = await speciesQuery.GetObjectsWithPagination(query.Page, query.PageSize, token);

        logger.Log(LogLevel.Information,
            "Get species with pagination Page: {Page}, PageSize: {PageSize}, TotalCount: {TotalCount}",
            result.Page, result.PageSize, result.TotalCount);

        return result;
    }

}