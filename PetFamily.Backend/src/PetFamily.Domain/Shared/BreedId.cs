using PetFamily.Domain.Models;

namespace PetFamily.Domain.Shared;

public class BreedId 
{
    public Guid Id { get; }
    private BreedId(Guid id) => Id = id;

    public static BreedId NewId() => new BreedId(Guid.NewGuid());
    public static BreedId Empty() => new BreedId(Guid.Empty);
    public static BreedId Create(Guid id) => new (id);

    public static implicit operator Guid(BreedId breedId) => breedId.Id;
}