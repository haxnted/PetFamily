using PetFamily.Application.Dto;

namespace PetFamily.Application.Database;

public interface IReadDbContext
{
    public IQueryable<VolunteerDto> Volunteers { get; }
    public IQueryable<PetDto> Pets { get; }
    public IQueryable<SpeciesDto> Species { get; }
    public IQueryable<BreedDto> Breeds { get; }
}