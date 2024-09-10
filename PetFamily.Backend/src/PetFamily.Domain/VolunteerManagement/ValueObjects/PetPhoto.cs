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

    public static Result<PetPhoto, Error> Create(FilePath filePath, bool isImageMain = false)
    {
        if (string.IsNullOrWhiteSpace(filePath.Path))
            return Errors.General.ValueIsInvalid("filePath");
        
        var extension = System.IO.Path.GetExtension(filePath.Path);
        
        if(Constants.SUPPORTED_IMAGES_EXTENSIONS.Contains(extension) == false)
            return Errors.General.ValueIsInvalid("filePath-extension");
        
        return new PetPhoto(filePath, isImageMain);
    }
}

