using PetFamily.Application.Features.VolunteerManagement.Queries.GetAllPetsWithPagination;
using PetFamily.Domain.VolunteerManagement.Enums;

namespace PetFamily.API.Controllers.Volunteers;

public record GetAllPetsWithPaginationRequest(
    int Page,
    int PageSize,
    string? SortBy,
    string? SortDirection,
    Guid? VolunteerId,
    string? NickName,
    Guid? SpeciesId,
    Guid? BreedId,
    string? Street,
    string? City,
    string? State,
    string? ZipCode,
    int? MinHeight,
    int? MaxHeight,
    int? MinWeight,
    int? MaxWeight,
    int? MinAge,
    int? MaxAge,
    HelpStatusPet? HelpStatus,
    bool? IsCastrated,
    bool? IsVaccinated)
{
    public GetAllPetsWithPaginationQuery ToQuery() =>
        new(
            Page,
            PageSize,
            SortBy,
            SortDirection,
            VolunteerId,
            NickName,
            SpeciesId,
            BreedId,
            Street,
            City,
            State,
            ZipCode,
            MinHeight,
            MaxHeight,
            MinWeight,
            MaxWeight,
            MinAge,
            MaxAge,
            HelpStatus,
            IsCastrated,
            IsVaccinated);
}