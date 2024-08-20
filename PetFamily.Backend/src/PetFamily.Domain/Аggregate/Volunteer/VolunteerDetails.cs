namespace PetFamily.Domain.Аggregate.Volunteer;

public record VolunteerDetails
{
    public IReadOnlyCollection<SocialLink> SocialLinks { get; }
    public IReadOnlyCollection<Requisite> Requisites { get; }

    private VolunteerDetails() { }

    public VolunteerDetails(IEnumerable<SocialLink> socialLinks,
        IEnumerable<Requisite> requisites)
    {
        SocialLinks = socialLinks.ToList();
        Requisites = requisites.ToList();
    }
}