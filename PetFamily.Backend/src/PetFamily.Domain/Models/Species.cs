using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models;

public class Species : Entity<SpeciesId>
{
    protected Species(SpeciesId id) : base(id){}

    private Species(SpeciesId id, TypeAnimal typeAnimal, List<Breed>? breeds) : base(id)
    {
        TypeAnimal = typeAnimal;
        Breeds = breeds;
    }
    
    public TypeAnimal TypeAnimal { get; } 
    public List<Breed>? Breeds { get; }

    public static Result<Species> Create(SpeciesId? id, TypeAnimal typeAnimal, List<Breed>? breeds)
    {
        if (id == null)
            return Result<Species>.Failure("SpeciesId cannot be null");
        
        return Result<Species>.Success(new Species(id, typeAnimal, breeds));
    }
}