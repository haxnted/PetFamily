using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Dto;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.VolunteerManagement.Queries.GetAllPetsWithPagination;

public class GetAllPetsWithPaginationHandler(
    IReadDbContext readDbContext) : IQueryHandler<PagedList<PetDto>, GetAllPetsWithPaginationQuery>
{
    public async Task<Result<PagedList<PetDto>, ErrorList>> Execute(GetAllPetsWithPaginationQuery query,
        CancellationToken token = default)
    {
        var petsQuery = ApplyFilters(readDbContext.Pets, query); 
        
        var keySelector = SortByProperty(query.SortBy);

        petsQuery = query.SortDirection?.ToLower() == "desc"
            ? petsQuery.OrderByDescending(keySelector)
            : petsQuery.OrderBy(keySelector);

        return await petsQuery.GetObjectsWithPagination(query.Page, query.PageSize, token);
    }
    
    private static IQueryable<PetDto> ApplyFilters(IQueryable<PetDto> petsQuery, GetAllPetsWithPaginationQuery query)
    {
        return petsQuery
            .WhereIf(!string.IsNullOrEmpty(query.NickName), pet => pet.NickName.Contains(query.NickName!))
            .WhereIf(query.MinAge.HasValue, pet => (DateTime.Now - pet.BirthDate).TotalDays / 365 >= query.MinAge)
            .WhereIf(query.MaxAge.HasValue, pet => (DateTime.Now - pet.BirthDate).TotalDays / 365 <= query.MaxAge)
            .WhereIf(query.SpeciesId.HasValue, pet => pet.SpeciesId == query.SpeciesId)
            .WhereIf(query.BreedId.HasValue, pet => pet.BreedId == query.BreedId)
            .WhereIf(!string.IsNullOrEmpty(query.City), pet => pet.Address.City == query.City)
            .WhereIf(!string.IsNullOrEmpty(query.Street), pet => pet.Address.Street == query.Street)
            .WhereIf(!string.IsNullOrEmpty(query.State), pet => pet.Address.State == query.State)
            .WhereIf(!string.IsNullOrEmpty(query.ZipCode), pet => pet.Address.ZipCode == query.ZipCode)
            .WhereIf(query.minHeight.HasValue, pet => pet.Height >= query.minHeight)
            .WhereIf(query.maxHeight.HasValue, pet => pet.Height <= query.maxHeight)
            .WhereIf(query.MinWeight.HasValue, pet => pet.Weight >= query.MinWeight)
            .WhereIf(query.MaxWeight.HasValue, pet => pet.Weight <= query.MaxWeight)
            .WhereIf(query.VolunteerId.HasValue, pet => pet.VolunteerId == query.VolunteerId)
            .WhereIf(query.IsCastrated.HasValue, pet => pet.IsCastrated == query.IsCastrated)
            .WhereIf(query.IsVaccinated.HasValue, pet => pet.IsVaccinated == query.IsVaccinated)
            .WhereIf(query.HelpStatus.HasValue, pet => pet.HelpStatus == query.HelpStatus);
    }
    private Expression<Func<PetDto, object>> SortByProperty(string? sortBy)
    {
        if (string.IsNullOrEmpty(sortBy))
            return volunteer => volunteer.Id;

        Expression<Func<PetDto, object>> keySelector = sortBy?.ToLower() switch
        {
            "nickname" => prop => prop.NickName,
            "street" => prop => prop.Address.Street,
            "city" => prop => prop.Address.City,
            "state" => prop => prop.Address.State,
            "zipcode" => prop => prop.Address.ZipCode,
            "birthdate" => prop => prop.BirthDate,
            "breed" => prop => prop.BreedId,
            "species" => prop => prop.SpeciesId,
            "volunteer" => prop => prop.VolunteerId,
            "height" => prop => prop.Height,
            "weight" => prop => prop.Weight,
            _ => prop => prop.NickName
        };
        return keySelector;
    }
}