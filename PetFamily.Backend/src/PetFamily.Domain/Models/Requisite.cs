using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models;

public record class Requisite
{
    public string RequisiteName { get; }
    public string RequisiteDescription { get; }

    private Requisite(string requisiteName, string requisiteDescription)
    {
        RequisiteName = requisiteName;
        RequisiteDescription = requisiteDescription;
    }

    public static Result<Requisite> Create(string requisiteName, string requisiteDescription)
    {
        if (string.IsNullOrWhiteSpace(requisiteName) || requisiteName.Length > Constants.MIDDLE_TEXT_LENGTH)
            return Result<Requisite>.Failure($"Requisite name cannot be empty or more then {Constants.MIDDLE_TEXT_LENGTH}. ");

        if (string.IsNullOrWhiteSpace(requisiteDescription) || requisiteDescription.Length > Constants.EXTRA_TEXT_LENGTH)
            return Result<Requisite>.Failure($"Requisite description cannot be empty or more then {Constants.EXTRA_TEXT_LENGTH}");

        return Result<Requisite>.Success(new Requisite(requisiteName, requisiteDescription));
    }
}