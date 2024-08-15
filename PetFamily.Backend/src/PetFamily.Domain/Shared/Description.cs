namespace PetFamily.Domain.Shared;

public record class Description
{
    public string Value { get; }

    private Description(string description)
    {
        Value = description;
    }

    public static Result<Description> Create(string description)
    {
        if (string.IsNullOrWhiteSpace(description) || description.Length > Constants.EXTRA_TEXT_LENGTH)
            return Result<Description>.Failure($"Description cannot be null or more then {Constants.EXTRA_TEXT_LENGTH}. ");

        return Result<Description>.Success(new Description(description));
    }
}