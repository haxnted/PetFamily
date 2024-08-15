using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models;

public record class PetPhoto
{
    public string Path { get; }
    public bool IsImageMain { get; }

    private PetPhoto(string path, bool isImageMain)
    {
        Path = path;
        IsImageMain = isImageMain;
    }

    public static Result<PetPhoto> Create(string path, bool isImageMain)
    {
        if (string.IsNullOrWhiteSpace(path))
            return Result<PetPhoto>.Failure("path cannot be empty");

        return Result<PetPhoto>.Success(new PetPhoto(path, isImageMain));
    }
}