using PetFamily.VolunteerManagement.Application.Queries.GetVolunteersWithPagination;

namespace PetFamily.VolunteerManagement.Presentation.Volunteers.Requests;

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