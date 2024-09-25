namespace PetFamily.Application.Dto;

public class SpeciesDto
{
    public Guid Id { get; init; }
    public BreedDto[] Breeds { get; init; } = [];
    public string TypeAnimal { get; init; } = string.Empty;
}