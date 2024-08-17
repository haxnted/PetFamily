using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models;

public class PetPhoto : Shared.Entity<PetPhotoId> 
{
    public string Path { get; }
    public bool IsImageMain { get; }

    private PetPhoto(PetPhotoId id,string path, bool isImageMain) : base(id)
    {
        Path = path;
        IsImageMain = isImageMain;
    }

    public static Result<PetPhoto, Error> Create(PetPhotoId id, string path, bool isImageMain)
    {
        if (string.IsNullOrWhiteSpace(path))
            return Errors.General.ValueIsInvalid("path cannot be empty");

        return new PetPhoto(id, path, isImageMain);
    }
}