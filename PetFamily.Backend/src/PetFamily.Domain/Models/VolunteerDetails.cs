namespace PetFamily.Domain.Models;

public record class VolunteerDetails
{
    public List<SocialLink> SocialLinks { get; }
    public List<Requisite> Requisites { get; }
}