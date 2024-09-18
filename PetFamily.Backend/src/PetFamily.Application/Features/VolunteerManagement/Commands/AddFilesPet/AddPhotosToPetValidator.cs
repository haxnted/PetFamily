using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.VolunteerManagement.Commands.AddFilesPet;

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
            .WithError(Errors.General.ValueIsRequired("File"));

        RuleForEach(p => p.Files)
            .SetValidator(new PetPhotoValidator());
    }
}

public class PetPhotoValidator : AbstractValidator<CreateFileCommand>
{
    public const int MAX_SIZE_FILE = 10 * 1024 * 1024; // 10 MB 
    public PetPhotoValidator()
    {
        RuleFor(p => p.Content.Length)
            .Must(x => x <= MAX_SIZE_FILE)
            .WithError(Errors.General.ValueIsInvalid("File size"));

        RuleFor(p => p.FileName)
            .Must(name =>
            {
                var fileExtension = Path.GetExtension(name);
                return Constants.SUPPORTED_IMAGES_EXTENSIONS.Contains(fileExtension) != false;
            })
            .WithError(Errors.General.ValueIsInvalid("FileType"));
    }
}