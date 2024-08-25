 namespace PetFamily.Domain.Аggregate.Volunteer;

public record RequisitesList
{
    public IReadOnlyCollection<Requisite> Requisites { get; }
    public RequisitesList(IEnumerable<Requisite> socialLinks) => Requisites = socialLinks.ToList();
};
