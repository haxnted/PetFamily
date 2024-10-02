using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteerManagement.ValueObjects;

namespace PetFamily.Application.Features.VolunteerManagement.Commands.UpdatePetPhotoMain;

public class UpdatePetPhotoMainValidator : AbstractValidator<UpdatePetPhotoMainCommand>
{
    public UpdatePetPhotoMainValidator()
    {
        RuleFor(p => p.VolunteerId)
            .NotEmpty().NotEmpty().WithError(Errors.General.ValueIsRequired("VolunteerId"));

        RuleFor(p => p.PetId)
            .NotEmpty().NotEmpty().WithError(Errors.General.ValueIsRequired("PetId"));

        RuleFor(p => p.FileName)
            .MustBeValueObject(FilePath.Create);
    }
}