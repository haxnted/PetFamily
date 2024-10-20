using CSharpFunctionalExtensions;

namespace PetFamily.SharedKernel.ValueObjects;

public record Requisite
{
    public string Name { get; }
    public string Description { get; }
    
    private Requisite(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public static Result<Requisite, Error> Create(string requisiteName, string requisiteDescription)
    {
        if (string.IsNullOrWhiteSpace(requisiteName) || requisiteName.Length > Constants.MIDDLE_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid($"Requisite");

        if (string.IsNullOrWhiteSpace(requisiteDescription) ||
            requisiteDescription.Length > Constants.EXTRA_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid($"Requisite");

        return new Requisite(requisiteName, requisiteDescription);
    }
}