using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models;

public class Pet
{
    protected Pet() { }
    public PetId Id { get; } = null!;
    public string NickName { get; } = string.Empty;
    public TypeAnimal TypeAnimal { get; }
    public Description GeneralDescription { get; } = null!;
    public string Breed { get; }  // Порода животного
    public string Color { get; } // Окрас животного
    public Description HealthInformation { get; }  
    public Address Address { get; }

    public PetPhysicalAttributes PhysicalAttributes { get; } = null!;
    public PhoneNumber PhoneNumber { get; } = null!;
    public DateOnly BirthDate { get; }
    public bool IsCastrated { get; } 
    public bool IsVaccinated { get; } 
    
    public HelpStatusPet HelpStatus { get; }
    public Requisite Requisite { get; }
    public DateTimeOffset DateCreated { get; }

    public PetDetails Details { get; }
}

