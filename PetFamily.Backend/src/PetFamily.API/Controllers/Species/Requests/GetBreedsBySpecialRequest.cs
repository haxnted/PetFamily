using PetFamily.Application.Features.Species.Queries.GetBreedsBySpecial;

namespace PetFamily.API.Controllers.SpeciesController.Requests;

public record GetBreedsBySpecialRequest(Guid SpecialId, string? SortDirection)
{
    public GetBreedsBySpecialQuery ToQuery() =>
        new(SpecialId, SortDirection);
}