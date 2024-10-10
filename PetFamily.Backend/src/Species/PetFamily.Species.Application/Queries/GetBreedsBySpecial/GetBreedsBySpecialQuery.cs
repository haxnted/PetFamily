using PetFamily.Core.Abstractions;

namespace PetFamily.Species.Application.Queries.GetBreedsBySpecial;

public record GetBreedsBySpecialQuery(Guid SpecialId, string? SortDirection) : IQuery;