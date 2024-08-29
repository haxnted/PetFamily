namespace PetFamily.Domain.VolunteerManagement;

public record SocialLinkList
{
    private SocialLinkList() { }
    public IReadOnlyCollection<SocialLink> SocialLinks { get; }
    public SocialLinkList(IEnumerable<SocialLink> socialLinks) => SocialLinks = socialLinks.ToList();
}