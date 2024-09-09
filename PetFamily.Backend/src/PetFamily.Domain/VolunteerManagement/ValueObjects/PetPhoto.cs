using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.VolunteerManagement.ValueObjects;

public record PetPhoto
{
    public FilePath Path { get; }
    public bool IsImageMain { get; }

    private PetPhoto(FilePath path, bool isImageMain = false)
    {
        Path = path;
        IsImageMain = isImageMain;
    }

    public static Result<PetPhoto, Error> Create(FilePath path, bool isImageMain = false)
    {
        if (string.IsNullOrWhiteSpace(path.Value))
            return Errors.General.ValueIsInvalid("path");
        
        var extension = System.IO.Path.GetExtension(path.Value);
        
        if(Constants.SUPPORTED_IMAGES_EXTENSIONS.Contains(extension) == false)
            return Errors.General.ValueIsInvalid("path-extension");
        
        return new PetPhoto(path, isImageMain);
    }
}

