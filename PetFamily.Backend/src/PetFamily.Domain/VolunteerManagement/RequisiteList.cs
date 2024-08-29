 namespace PetFamily.Domain.VolunteerManagement;

public record RequisiteList
{
    private RequisiteList(){}
    public IReadOnlyCollection<Requisite> Requisites { get; }
    public RequisiteList(IEnumerable<Requisite> requisites) => Requisites = requisites.ToList();
};
