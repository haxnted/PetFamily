using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models;

public record PetDetails
{
    private readonly List<PetPhoto> _photos = [];
    public IReadOnlyCollection<PetPhoto> PetPhotos => _photos;
    private PetDetails(List<PetPhoto> photos) =>
        _photos = photos;
    public void AddPhoto(PetPhoto photo) =>
        _photos.Add(photo);

    public static Result<PetDetails> Create(List<PetPhoto> photos)
    {
        photos ??= [];
        return new PetDetails(photos);
    }
}