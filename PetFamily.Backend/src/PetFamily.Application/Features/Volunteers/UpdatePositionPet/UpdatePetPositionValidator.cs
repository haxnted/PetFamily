using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.UpdatePositionPet;

public class UpdatePetPositionValidator : AbstractValidator<UpdatePetPositionCommand>
{
    public UpdatePetPositionValidator()
    {
        RuleFor(p => p.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired("VolunteerId"));
        
        RuleFor(p => p.PetId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired("PetId"));

        RuleFor(p => p.Position)
            .Must(p => p <= 0)
            .WithError(Errors.General.ValueIsInvalid("Position"));
    }
}