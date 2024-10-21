using PetFamily.SharedKernel.ValueObjects;
namespace PetFamily.Accounts.Domain;

public class VolunteerAccount
{
    public Guid Id { get; set; }
    public FullName FullName { get; set; }
    public int Experience { get; set; }
    public List<Requisite> Requisites { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
}