namespace PetFamily.Domain.VolunteerManagement;

public record SocialLinksList
{
    private SocialLinksList() { }
    public IReadOnlyCollection<SocialLink> SocialLinks { get; }
    public SocialLinksList(IEnumerable<SocialLink> socialLinks) => SocialLinks = socialLinks.ToList();
}