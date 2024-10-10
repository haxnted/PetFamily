namespace PetFamily.Core.Dto;

public class SpeciesDto
{
    public Guid Id { get; init; }
    public IEnumerable<BreedDto> Breeds { get; init; }
    public string TypeAnimal { get; init; } = string.Empty;
}