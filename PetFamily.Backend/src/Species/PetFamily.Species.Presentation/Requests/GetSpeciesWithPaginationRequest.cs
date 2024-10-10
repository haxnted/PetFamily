using PetFamily.Species.Application.Queries.GetAllSpeciesWithPagination;

namespace PetFamily.Species.Presentation.Requests;

public record GetSpeciesWithPaginationRequest(
    string? SortDirection,
    int Page,
    int PageSize)
{
    public GetSpeciesWithPaginationQuery ToQuery() =>
        new(SortDirection, Page, PageSize);
}