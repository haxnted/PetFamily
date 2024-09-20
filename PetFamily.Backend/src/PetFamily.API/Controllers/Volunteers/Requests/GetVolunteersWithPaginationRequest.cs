using PetFamily.Application.Features.VolunteerManagement.Queries.GetVolunteersWithPagination;

namespace PetFamily.API.Controllers.Volunteers.Requests;

public record GetVolunteersWithPaginationRequest(
    string? SortBy,
    string? SortDirection,
    int Page,
    int PageSize)
{
    public GetVolunteersWithPaginationQuery ToQuery() =>
        new(
            SortBy,
            SortDirection,
            Page,
            PageSize);
}