using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Shared;

public record Description
{
    public string Value { get; }

    private Description(string description)
    {
        Value = description;
    }

    public static Result<Description, Error> Create(string description)
    {
        if (string.IsNullOrWhiteSpace(description) || description.Length > Constants.EXTRA_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid($"Description cannot be null or more then {Constants.EXTRA_TEXT_LENGTH}. ");

        return new Description(description);
    }
}