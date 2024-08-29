using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.VolunteerManagement;

namespace PetFamily.Domain.Species;

public class Species : Shared.Entity<SpeciesId>
{
    protected Species(SpeciesId id) : base(id) { }

    public Species(SpeciesId id, TypeAnimal typeAnimal, IEnumerable<Breed> breeds) : base(id)
    {
        TypeAnimal = typeAnimal;
        Breeds = breeds.ToList();
    }

    public TypeAnimal TypeAnimal { get; }
    public List<Breed> Breeds { get; }
}