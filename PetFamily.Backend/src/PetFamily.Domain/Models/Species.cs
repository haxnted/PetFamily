using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models;

public class Species : Shared.Entity<SpeciesId>
{
    protected Species(SpeciesId id) : base(id){}

    private Species(SpeciesId id, TypeAnimal typeAnimal, List<Breed>? breeds) : base(id)
    {
        TypeAnimal = typeAnimal;
        Breeds = breeds ?? [];
    }
    
    public TypeAnimal TypeAnimal { get; } 
    public List<Breed>? Breeds { get; }

    public static Result<Species, Error> Create(SpeciesId? id, TypeAnimal typeAnimal, List<Breed>? breeds)
    {
        if (id == null)
            return Errors.General.ValueIsInvalid("SpeciesId cannot be null");
        
        return new Species(id, typeAnimal, breeds);
    }
}