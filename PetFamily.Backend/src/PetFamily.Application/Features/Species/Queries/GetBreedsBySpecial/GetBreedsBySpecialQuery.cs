using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Features.Species.Queries.GetBreedsBySpecial;

public record GetBreedsBySpecialQuery(Guid SpecialId, string? SortDirection) : IQuery;