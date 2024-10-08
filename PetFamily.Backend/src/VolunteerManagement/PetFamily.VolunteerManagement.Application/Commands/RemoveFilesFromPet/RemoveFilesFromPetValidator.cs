using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.VolunteerManagement.Application.Commands.RemoveFilesFromPet;

public class RemoveFilesFromPetValidator : AbstractValidator<RemoveFilesFromPetCommand>
{
    public RemoveFilesFromPetValidator()
    {
        RuleFor(v => v.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired("VolunteerId"));

        RuleFor(v => v.PetId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired("PetId"));
    }
}