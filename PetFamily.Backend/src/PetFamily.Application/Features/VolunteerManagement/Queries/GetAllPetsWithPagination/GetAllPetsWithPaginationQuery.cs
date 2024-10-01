using PetFamily.Application.Abstractions;
using PetFamily.Domain.VolunteerManagement.Enums;

namespace PetFamily.Application.Features.VolunteerManagement.Queries.GetAllPetsWithPagination;

public record GetAllPetsWithPaginationQuery(
    int Page,
    int PageSize,
    string? SortBy,
    string? SortDirection,
    Guid? VolunteerId = null,
    string? NickName = null,
    Guid? SpeciesId = null,
    Guid? BreedId = null,
    string? Street = null,
    string? City = null,
    string? State = null,
    string? ZipCode = null,
    int? minHeight = null,
    int? maxHeight = null,
    int? MinWeight = null,
    int? MaxWeight = null,
    int? MinAge = null,
    int? MaxAge = null,
    HelpStatusPet? HelpStatus = null,
    bool? IsCastrated = null,
    bool? IsVaccinated = null) : IQuery;