using System.Text.RegularExpressions;

namespace PetFamily.Domain.Shared;

public record class PhoneNumber
{
    private const string phoneRegex = @"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\-]?)?[\d\-]{7,10}$";
    public string Value { get; }
    private PhoneNumber(string number) => Value = number;

    public static Result<PhoneNumber> Create(string number)
    {
        if (string.IsNullOrWhiteSpace(number))
            return Result<PhoneNumber>.Failure("Number cannot be null");

        if (!Regex.IsMatch(number, phoneRegex))
            return Result<PhoneNumber>.Failure("Phone number is incorrect");
        
        return Result<PhoneNumber>.Success(new PhoneNumber(number));
    } 
    
}