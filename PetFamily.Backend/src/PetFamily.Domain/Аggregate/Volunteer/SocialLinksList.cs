namespace PetFamily.Domain.Аggregate.Volunteer;

public record SocialLinksList
{
    private SocialLinksList() { }
    public IReadOnlyCollection<SocialLink> SocialLinks { get; }
    public SocialLinksList(IEnumerable<SocialLink> socialLinks) => SocialLinks = socialLinks.ToList();
}