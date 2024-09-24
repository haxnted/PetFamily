using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Features.Species.Queries.GetAllSpeciesWithPagination;

public record GetSpeciesWithPaginationQuery(
    string? SortDirection,
    int Page,
    int PageSize) : IQuery;