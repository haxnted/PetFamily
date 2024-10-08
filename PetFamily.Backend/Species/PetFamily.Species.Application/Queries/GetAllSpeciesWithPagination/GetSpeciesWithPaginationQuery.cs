using PetFamily.Core.Abstractions;

namespace PetFamily.Species.Application.Queries.GetAllSpeciesWithPagination;

public record GetSpeciesWithPaginationQuery(
    string? SortDirection,
    int Page,
    int PageSize) : IQuery;