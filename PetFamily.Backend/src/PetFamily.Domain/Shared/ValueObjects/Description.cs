using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Shared.ValueObjects;

public record Description
{
    public string Value { get; }

    private Description(string value) => Value = value;

    public static Result<Description, Error> Create(string description)
    {
        if (string.IsNullOrWhiteSpace(description) || description.Length > Constants.EXTRA_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid("Description");

        return new Description(description);
    }
}