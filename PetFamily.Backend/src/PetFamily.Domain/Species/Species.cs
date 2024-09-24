using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.Species.Entities;
using PetFamily.Domain.VolunteerManagement.ValueObjects;

namespace PetFamily.Domain.Species;

public class Species : Shared.Entity<SpeciesId>
{
    protected Species(SpeciesId id) : base(id) { }

    public Species(SpeciesId id, TypeAnimal typeAnimal, IEnumerable<Breed> breeds) : base(id)
    {
        TypeAnimal = typeAnimal;
        _breeds = breeds.ToList();
    }

    private List<Breed> _breeds = [];
    public TypeAnimal TypeAnimal { get; }
    public IReadOnlyList<Breed> Breeds => _breeds;


    public UnitResult<Error> AddBreed(Breed breed)
    {
        var breedExists = _breeds.Any(b => b == breed);
        if (breedExists)
            return UnitResult.Failure(Errors.Model.AlreadyExist("breed"));
        
        _breeds.Add(breed);
        return UnitResult.Success<Error>();
    }

    public void RemoveBreed(Breed breed)
    {
        _breeds.Remove(breed);
    }
}