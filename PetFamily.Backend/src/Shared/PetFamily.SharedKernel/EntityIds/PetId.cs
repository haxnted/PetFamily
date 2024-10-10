namespace PetFamily.SharedKernel.EntityIds;

public record PetId
{
    public Guid Id { get; }

    private PetId(Guid id) => Id = id;

    public static PetId NewId() => new (Guid.NewGuid());
    public static PetId Create(Guid id) => new (id);
    public static implicit operator Guid(PetId breedId) => breedId.Id;
}