using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.VolunteerManagement.Application.Commands.UpdatePositionPet;

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