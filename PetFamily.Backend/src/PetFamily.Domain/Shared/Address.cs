using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Shared;

public record Address
{
    public string Street { get; }
    public string City { get; }
    public string State { get; }
    public string ZipCode { get; }

    private Address(string street, string city, string state, string zipCode)
    {
        Street = street;
        City = city;
        State = state;
        ZipCode = zipCode;
    }
    public static Result<Address, Error> Create(string street, string city, string state, string zipCode)
    {
        if (string.IsNullOrWhiteSpace(street) || street.Length > Constants.MIN_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid($"Street cannot be empty or more then {Constants.MIN_TEXT_LENGTH}.");

        if (string.IsNullOrWhiteSpace(city) || city.Length > Constants.MIN_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid($"City cannot be empty or more then {Constants.MIN_TEXT_LENGTH}.");

        if (string.IsNullOrWhiteSpace(state) || state.Length > Constants.MIN_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid($"State cannot be empty or more then {Constants.MIN_TEXT_LENGTH}.");

        if (string.IsNullOrWhiteSpace(zipCode) || zipCode.Length > Constants.MIN_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid($"ZipCode cannot be empty or more then {Constants.MIN_TEXT_LENGTH}.");

        return new Address(street, city, state, zipCode);
    }
}
