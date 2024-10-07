using PetFamily.Core.Dto;

namespace PetFamily.Species.Application;

public interface ISpeciesReadDbContext
{
    public IQueryable<SpeciesDto> Species { get; }
    public IQueryable<BreedDto> Breeds { get; }
}
