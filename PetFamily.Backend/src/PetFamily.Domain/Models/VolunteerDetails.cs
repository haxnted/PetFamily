using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models;

public record VolunteerDetails
{
    private readonly List<SocialLink> _socialLinks = [];
    private readonly List<Requisite> _requisites = [];
    public IReadOnlyCollection<SocialLink> SocialLinks => _socialLinks;
    public IReadOnlyCollection<Requisite> Requisites => _requisites;
    
    protected VolunteerDetails(){}
    private VolunteerDetails(List<SocialLink> socialLinks,
        List<Requisite> requisites)
    {
        _socialLinks = socialLinks;
        _requisites = requisites;
    }
    public static Result<VolunteerDetails> Create(List<SocialLink>? socialLinks,
        List<Requisite>? requisites)
    {
        socialLinks ??= [];
        requisites ??= [];

        return new VolunteerDetails(socialLinks, requisites);
    }
    
    public void AddRequisite(Requisite socialLink) =>
        _requisites.Add(socialLink);
    
    public void AddSocialLink(SocialLink socialLink) =>
        _socialLinks.Add(socialLink);
    
}