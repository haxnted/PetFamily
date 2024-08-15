using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models;

public class Breed : Entity<BreedId>
{
    public string Value { get; set; }
    
    protected Breed(BreedId id) : base(id){}

    private Breed(BreedId id, string breed) : base(id)
    {
        Value = breed;
    }
    public static Result<Breed> Create(BreedId id, string breed)
    {
        if (id == null)
            return Result<Breed>.Failure("BreedId cannot be null");
        
        if (string.IsNullOrWhiteSpace(breed) || breed.Length > Constants.MIN_TEXT_LENGTH)
            return Result<Breed>.Failure($"breed cannot be empty or more then {Constants.MIN_TEXT_LENGTH}. ");

        return Result<Breed>.Success(new Breed(id, breed));
    }
}