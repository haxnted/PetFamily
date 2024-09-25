using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;

namespace PetFamily.Domain.Species.Entities;

public class Breed : Shared.Entity<BreedId>
{
    
    public string Value { get; }
    
    public SpeciesId SpeciesId { get; }
    protected Breed(BreedId id) : base(id) { }

    private Breed(BreedId id, string breed) : base(id)
    {
        Value = breed;
    }
    
    public static Result<Breed, Error> Create(BreedId id, string breed)
    {
        if (string.IsNullOrWhiteSpace(breed) || breed.Length > Constants.MIN_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid($"breed");
        
        if (!Regex.IsMatch(breed, @"^[а-яА-ЯёЁ]+$"))
            return Errors.General.ValueIsInvalid("breed");
        
        return (new Breed(id, breed.ToLower()));
    }
}