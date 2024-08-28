 namespace PetFamily.Domain.VolunteerManagement;

public record RequisitesList
{
    private RequisitesList(){}
    public IReadOnlyCollection<Requisite> Requisites { get; }
    public RequisitesList(IEnumerable<Requisite> requisites) => Requisites = requisites.ToList();
};
