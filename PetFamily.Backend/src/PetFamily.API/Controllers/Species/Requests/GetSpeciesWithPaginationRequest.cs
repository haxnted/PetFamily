using PetFamily.Application.Features.Species.Queries.GetAllSpeciesWithPagination;

namespace PetFamily.API.Controllers.SpeciesController.Requests;

public record GetSpeciesWithPaginationRequest(
    string? SortDirection,
    int Page,
    int PageSize)
{
    public GetSpeciesWithPaginationQuery ToQuery() =>
        new(SortDirection, Page, PageSize);
}