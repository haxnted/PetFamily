namespace PetFamily.Application.Dto;

public class BreedDto
{
    public Guid Id { get; init; }
    public Guid SpeciesId { get; init; }
    
    public string Name { get; init; } = string.Empty;
}