using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.VolunteerManagement;

public record class Requisite
{
    public string RequisiteName { get; }
    public string RequisiteDescription { get; }

    private Requisite(string requisiteName, string requisiteDescription)
    {
        RequisiteName = requisiteName;
        RequisiteDescription = requisiteDescription;
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