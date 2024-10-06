using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dto;
using PetFamily.Core.Extensions;
using PetFamily.Core.Models;
using PetFamily.SharedKernel;

namespace PetFamily.VolunteerManagement.Application.Queries.GetVolunteersWithPagination;

public class GetVolunteersWithPaginationHandler(
    IVolunteersReadDbContext readDbContext,
    IValidator<GetVolunteersWithPaginationQuery> validator,
    ILogger<GetVolunteersWithPaginationHandler> logger)
    : IQueryHandler<PagedList<VolunteerDto>, GetVolunteersWithPaginationQuery>
{
    public async Task<Result<PagedList<VolunteerDto>, ErrorList>> Execute(
        GetVolunteersWithPaginationQuery query,
        CancellationToken token = default
    )
    {
        var validationResult = await validator.ValidateAsync(query, token);
        if (validationResult.IsValid == false)
            return validationResult.ToList();

        var volunteersQuery = readDbContext.Volunteers;

        var keySelector = SortByProperty(query.SortBy);

        volunteersQuery = query.SortDirection?.ToLower() == "desc"
            ? volunteersQuery.OrderByDescending(keySelector)
            : volunteersQuery.OrderBy(keySelector);

        var result = await volunteersQuery.GetObjectsWithPagination(query.Page, query.PageSize, token);

        logger.Log(LogLevel.Information,
            "Get volunteers with pagination Page: {Page}, PageSize: {PageSize}, TotalCount: {TotalCount}",
            result.Page, result.PageSize, result.TotalCount);

        return result;
    }

    private static Expression<Func<VolunteerDto, object>> SortByProperty(string? sortBy)
    {
        if (string.IsNullOrEmpty(sortBy))
            return volunteer => volunteer.Id;

        Expression<Func<VolunteerDto, object>> keySelector = sortBy?.ToLower() switch
        {
            "name" => volunteer => volunteer.FullName.Name,
            "surname" => volunteer => volunteer.FullName.Surname,
            "age" => volunteer => volunteer.AgeExperience,
            _ => volunteer => volunteer.Id
        };
        return keySelector;
    }
}