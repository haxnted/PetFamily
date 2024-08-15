namespace PetFamily.Domain.Shared;

public record Address
{
    public string Street { get; }
    public string City { get; }
    public string State { get; } // Область, регион
    public string ZipCode { get; } // Почтовый индекс

    private Address(string street, string city, string state, string zipCode)
    {
        Street = street;
        City = city;
        State = state;
        ZipCode = zipCode;
    }
    public static Result<Address> Create(string street, string city, string state, string zipCode)
    {
        if (string.IsNullOrWhiteSpace(street) || street.Length > Constants.MIN_TEXT_LENGTH)
            return Result<Address>.Failure($"Street cannot be empty or more then {Constants.MIN_TEXT_LENGTH}.");

        if (string.IsNullOrWhiteSpace(city) || city.Length > Constants.MIN_TEXT_LENGTH)
            return Result<Address>.Failure($"City cannot be empty or more then {Constants.MIN_TEXT_LENGTH}.");

        if (string.IsNullOrWhiteSpace(state) || state.Length > Constants.MIN_TEXT_LENGTH)
            return Result<Address>.Failure($"State cannot be empty or more then {Constants.MIN_TEXT_LENGTH}.");

        if (string.IsNullOrWhiteSpace(zipCode) || zipCode.Length > Constants.MIN_TEXT_LENGTH)
            return Result<Address>.Failure($"ZipCode cannot be empty or more then {Constants.MIN_TEXT_LENGTH}.");

        return Result<Address>.Success(new Address(street, city, state, zipCode));
    }
}
