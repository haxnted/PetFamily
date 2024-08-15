using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models;

public class Pet : Entity<PetId>
{
    protected Pet(PetId id): base(id) { }
    public string NickName { get; } = string.Empty;
    public Description GeneralDescription { get; } = null!;
    public Description HealthInformation { get; } = null!;
    
    public BreedId BreedId { get; } = null!;
    
    public SpeciesId SpeciesId { get; } = null!;
    public Address Address { get; } = null!;
    public PetPhysicalAttributes PhysicalAttributes { get; } = null!;
    public PhoneNumber PhoneNumber { get; } = null!;
    public DateOnly BirthDate { get; }
    public bool IsCastrated { get; } 
    public bool IsVaccinated { get; } 
    
    public HelpStatusPet HelpStatus { get; }
    public Requisite Requisite { get; } = null!;
    public DateTimeOffset DateCreated { get; }
    public PetDetails Details { get; } = null!;
}

