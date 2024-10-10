using PetFamily.Species.Application.Queries.GetBreedsBySpecial;

namespace PetFamily.Species.Presentation.Requests;

public record GetBreedsBySpecialRequest(Guid SpecialId, string? SortDirection)
{
    public GetBreedsBySpecialQuery ToQuery() =>
        new(SpecialId, SortDirection);
}