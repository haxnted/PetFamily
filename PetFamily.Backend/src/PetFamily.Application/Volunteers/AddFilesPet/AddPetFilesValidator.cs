using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.AddFilesPet;

public class AddPetFilesValidator : AbstractValidator<AddPetFilesCommand>
{
    public AddPetFilesValidator()
    {
        RuleFor(p => p.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired("VolunteerId"));

        RuleFor(p => p.PetId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired("PetId"));

        RuleForEach(p => p.Files)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired("file"));

        RuleFor(p => new { p.IdxMainImage, p.Files })
            .Must(p => p.IdxMainImage < 0 || p.Files.Count() > p.IdxMainImage)
            .WithError(Errors.General.ValueIsInvalid("IdxMainImage"));
    }
}