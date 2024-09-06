using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.AddFilesPet;

public class AddPhotosToPetValidator : AbstractValidator<AddPhotosToPetCommand>
{
    public AddPhotosToPetValidator()
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
    }
}