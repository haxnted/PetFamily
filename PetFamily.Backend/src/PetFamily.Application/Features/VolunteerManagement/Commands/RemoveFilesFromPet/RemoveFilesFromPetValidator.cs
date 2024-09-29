using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.VolunteerManagement.Commands.RemoveFilesFromPet;

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