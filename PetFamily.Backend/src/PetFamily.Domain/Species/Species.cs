using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.VolunteerManagement;

namespace PetFamily.Domain.Species;

public class Species : Shared.Entity<SpeciesId>
{
    protected Species(SpeciesId id) : base(id) { }

    private Species(SpeciesId id, TypeAnimal typeAnimal, IEnumerable<Breed>? breeds) : base(id)
    {
        TypeAnimal = typeAnimal;
        Breeds = breeds?.ToList();
    }

    public TypeAnimal TypeAnimal { get; }
    public List<Breed>? Breeds { get; }

    public static Result<Species, Error> Create(SpeciesId? id, TypeAnimal typeAnimal, IEnumerable<Breed>? breeds)
    {
        if (id == null)
            return Errors.General.ValueIsInvalid("SpeciesId cannot be null");

        return new Species(id, typeAnimal, breeds);
    }
}