using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models;

public record PetDetails
{
    private PetDetails(){}
    
    private readonly List<Requisite> _requisites = [];
    public IReadOnlyCollection<Requisite> Requisites => _requisites;

    private readonly List<PetPhoto> _photos = [];
    public IReadOnlyCollection<PetPhoto> PetPhotos => _photos;
    private PetDetails(List<Requisite> requisites) =>
        _requisites = requisites ??= [];
    public void AddPhoto(Requisite requisite) =>
        _requisites.Add(requisite);

    public void AddRequisite(Requisite requisite) => 
        _requisites.Add(requisite);

    public static Result<PetDetails> Create(List<Requisite> requisites)
    {
        return new PetDetails(requisites);
    }
}