namespace PetFamily.Domain.Models;

public record class PetDetails
{
    public List<PetPhoto> Photos { get; }
}