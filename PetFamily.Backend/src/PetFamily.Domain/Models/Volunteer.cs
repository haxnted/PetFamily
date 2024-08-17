using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models;

public class Volunteer : Shared.Entity<VolunteerId>
{
    protected Volunteer(VolunteerId id) : base(id) {}

    private Volunteer(
        VolunteerId id, 
        FullName fullName, 
        Description generalDescription,
        AgeExperience ageExperience,
        PhoneNumber number,
        List<Pet> pets,
        VolunteerDetails details) : base(id)
    {
        FullName = fullName;
        GeneralDescription = generalDescription;
        AgeExperience = ageExperience;
        PhoneNumber = number;
        _pets = pets;
        Details = details;
        
    }
    public FullName FullName { get; }
    public Description GeneralDescription { get; }
    public AgeExperience AgeExperience { get; }
    public PhoneNumber PhoneNumber { get; }
    private readonly List<Pet> _pets = [];
    public IReadOnlyList<Pet> Pets => _pets;
    public VolunteerDetails? Details { get; } 

    public void AddPet(Pet pet) =>
        _pets.Add(pet);
    
    public static Result<Volunteer> Create(
        VolunteerId id,
        FullName fullName,
        Description generalDescription,
        AgeExperience ageExperience,
        PhoneNumber number,
        List<Pet> pets,
        VolunteerDetails details
    )
    {
        pets ??= [];
        return new Volunteer(id, fullName, generalDescription, 
                ageExperience, number, pets, details);
    }
    public int PetsAdoptedCount() =>
       Pets.Count(x => x.HelpStatus == HelpStatusPet.FoundHome);
    
    public int PetsFoundHomeQuantity() =>
     Pets.Count(x => x.HelpStatus == HelpStatusPet.LookingForHome);
    
    public int PetsUnderTreatmentCount() => 
         Pets.Count(x => x.HelpStatus == HelpStatusPet.NeedsHelp);
    
}
