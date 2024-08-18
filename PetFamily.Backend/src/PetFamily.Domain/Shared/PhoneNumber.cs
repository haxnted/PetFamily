using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Shared;

public record PhoneNumber
{
    private const string PhoneRegex = @"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\-]?)?[\d\-]{7,10}$";
    public string Value { get; }
    private PhoneNumber(){}
    private PhoneNumber(string number) => Value = number;

    public static Result<PhoneNumber, Error> Create(string number)
    {
        if (string.IsNullOrWhiteSpace(number))
            return Errors.General.ValueIsInvalid("Number cannot be null");

        if (Regex.IsMatch(number, PhoneRegex) == false)
            return Errors.General.ValueIsInvalid("Phone number is incorrect");
        
        return new PhoneNumber(number);
    } 
    
}