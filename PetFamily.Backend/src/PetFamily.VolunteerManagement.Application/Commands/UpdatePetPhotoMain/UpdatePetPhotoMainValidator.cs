using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.VolunteerManagement.Application.Commands.UpdatePetPhotoMain;

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