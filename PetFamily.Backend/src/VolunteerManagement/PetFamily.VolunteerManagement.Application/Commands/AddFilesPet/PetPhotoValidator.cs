using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.VolunteerManagement.Application.Commands.AddFilesPet;

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