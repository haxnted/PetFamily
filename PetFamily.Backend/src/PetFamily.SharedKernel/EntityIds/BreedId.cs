namespace PetFamily.SharedKernel.EntityIds;

public record BreedId
{
    public Guid Id { get; }
    
    private BreedId(Guid id) => Id = id;
    
    public static BreedId NewId() => new (Guid.NewGuid());
    public static BreedId Create(Guid id) => new(id);
    public static implicit operator Guid(BreedId breedId) => breedId.Id;
}