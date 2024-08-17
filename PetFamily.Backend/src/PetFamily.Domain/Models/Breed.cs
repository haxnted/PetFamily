using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models;

public class Breed : Shared.Entity<BreedId>
{
    public string Value { get; set; }
    
    protected Breed(BreedId id) : base(id){}

    private Breed(BreedId id, string breed) : base(id)
    {
        Value = breed;
    }
    public static Result<Breed, Error> Create(BreedId? id, string breed)
    {
        if (id == null)
            return Errors.General.ValueIsInvalid("BreedId cannot be null");

        if (string.IsNullOrWhiteSpace(breed) || breed.Length > Constants.MIN_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid($"breed cannot be empty or more then {Constants.MIN_TEXT_LENGTH}.");

        return (new Breed(id, breed));
    }
}