﻿namespace PetFamily.Domain.Shared;

public record SpeciesId
{
    public Guid Id { get; }

    private SpeciesId(Guid id) => Id = id;

    public static SpeciesId NewId() => new SpeciesId(Guid.NewGuid());
    public static SpeciesId Empty() => new SpeciesId(Guid.Empty);
    public static SpeciesId Create(Guid id) => new (id);
    public static implicit operator Guid(SpeciesId breedId) => breedId.Id;
}