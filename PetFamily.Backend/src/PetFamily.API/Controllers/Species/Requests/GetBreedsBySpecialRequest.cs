using PetFamily.Application.Features.Species.Queries.GetBreedsBySpecial;

namespace PetFamily.API.Controllers.Species.Requests;

public record GetBreedsBySpecialRequest(Guid SpecialId, string? SortDirection)
{
    public GetBreedsBySpecialQuery ToQuery() =>
        new(SpecialId, SortDirection);
}