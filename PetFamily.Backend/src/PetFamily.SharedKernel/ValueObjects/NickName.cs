using CSharpFunctionalExtensions;

namespace PetFamily.SharedKernel.ValueObjects;

public record NickName
{
    private NickName(string value) => Value = value;
    public string Value { get; }

    public static Result<NickName, Error> Create(string name)
    {
        if (string.IsNullOrEmpty(name))
            return Errors.General.ValueIsInvalid("NickName");

        return new NickName(name);
    }
}