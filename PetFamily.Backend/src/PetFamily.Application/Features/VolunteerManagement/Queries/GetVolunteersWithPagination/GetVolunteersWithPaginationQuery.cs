using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Features.VolunteerManagement.Queries.GetVolunteersWithPagination;

public record GetVolunteersWithPaginationQuery(
    string? SortBy,
    string? SortDirection,
    int Page,
    int PageSize) : IQuery;