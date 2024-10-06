using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;

namespace PetFamily.VolunteerManagement.Domain.ValueObjects;

public record TypeAnimal
{
    private TypeAnimal(string value) => Value = value;
    
    public string Value { get; }

    public static Result<TypeAnimal, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.General.ValueIsInvalid(value);
        
        if (!Regex.IsMatch(value, @"^[а-яА-ЯёЁ]+$"))
            return Errors.General.ValueIsInvalid("TypeAnimal");
        
        return new TypeAnimal(value.ToLower());
    }
}
