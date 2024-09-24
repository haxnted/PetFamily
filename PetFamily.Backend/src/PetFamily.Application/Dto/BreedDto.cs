namespace PetFamily.Application.Dto;

public class BreedDto
{
    public Guid Id { get; set; }
    public Guid SpeciesId { get; set; }
    
    public string Name { get; init; } = string.Empty;
}