using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.VolunteerManagement.Commands.RemoveSoftPetById;

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