using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models;

public record PetPhoto
{
    public string Path { get; }
    public bool IsImageMain { get; }

    private PetPhoto(string path, bool isImageMain)
    {
        Path = path;
        IsImageMain = isImageMain;
    }

    public static Result<PetPhoto, Error> Create(string path, bool isImageMain)
    {
        if (string.IsNullOrWhiteSpace(path))
            return Errors.General.ValueIsInvalid("path cannot be empty");

        return new PetPhoto(path, isImageMain);
    }
}