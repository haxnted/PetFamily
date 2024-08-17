namespace PetFamily.Domain.Shared;

public record PetId
{
    public Guid Id { get; }

    private PetId(Guid id) => Id = id;

    public static PetId NewId() => new PetId(Guid.NewGuid());
    public static PetId Empty() => new PetId(Guid.Empty);
    public static PetId Create(Guid id) => new (id);
    public static implicit operator Guid(PetId breedId) => breedId.Id;
}