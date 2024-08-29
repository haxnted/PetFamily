namespace PetFamily.Domain.VolunteerManagement;

public record PetPhotoList
{
    private PetPhotoList() { }

    public PetPhotoList(IEnumerable<PetPhoto> list) => PetPhotos = list.ToList();
    public IReadOnlyCollection<PetPhoto> PetPhotos { get; }
}