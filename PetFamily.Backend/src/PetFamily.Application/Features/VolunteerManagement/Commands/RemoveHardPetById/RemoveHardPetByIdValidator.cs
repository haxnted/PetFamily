using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.VolunteerManagement.Commands.RemoveHardPetById;

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