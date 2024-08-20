namespace PetFamily.Domain.Shared.EntityIds;

public record VolunteerId
{
    public Guid Id { get; }

    private VolunteerId(Guid id) => Id = id;

    public static VolunteerId NewId() => new (Guid.NewGuid());
    public static VolunteerId Empty() => new (Guid.Empty);
    public static VolunteerId Create(Guid id) => new (id);
    public static implicit operator Guid(VolunteerId breedId) => breedId.Id;
}