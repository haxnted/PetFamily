using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.VolunteerManagement.Application.Commands.RemoveHardPetById;

public class RemoveHardPetByIdValidator : AbstractValidator<RemoveHardPetByIdCommand>
{
    public RemoveHardPetByIdValidator()
    {
        RuleFor(x => x.VolunteerId)
            .NotEmpty().WithError(Errors.General.ValueIsInvalid());

        RuleFor(x => x.PetId)
            .NotEmpty().WithError(Errors.General.ValueIsInvalid());
    }
}