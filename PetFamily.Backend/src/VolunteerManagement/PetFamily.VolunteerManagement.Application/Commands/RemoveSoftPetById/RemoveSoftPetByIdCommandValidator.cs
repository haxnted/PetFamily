using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.VolunteerManagement.Application.Commands.RemoveSoftPetById;

public class RemoveSoftPetByIdCommandValidator : AbstractValidator<RemoveSoftPetByIdCommand>
{
    public RemoveSoftPetByIdCommandValidator()
    {
        RuleFor(x => x.VolunteerId)
            .NotEmpty().WithError(Errors.General.ValueIsInvalid("VolunteerId"));

        RuleFor(x => x.PetId)
            .NotEmpty().WithError(Errors.General.ValueIsInvalid("PetId"));
    }
}