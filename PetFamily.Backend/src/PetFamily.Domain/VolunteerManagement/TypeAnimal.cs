using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.VolunteerManagement;

public record TypeAnimal
{
    private TypeAnimal(string value) => Value = value;
    
    public string Value { get; }

    public static Result<TypeAnimal, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.General.ValueIsInvalid(value);
        
        return new TypeAnimal(value);
    }
}
