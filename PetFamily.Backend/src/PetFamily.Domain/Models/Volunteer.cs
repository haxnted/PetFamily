using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models;

public class Volunteer : Entity<VolunteerId>
{
    protected Volunteer(VolunteerId id) : base(id) {}

    private Volunteer(
        VolunteerId id, 
        FullName fullName, 
        Description generalDescription,
        AgeExperience ageExperience,
        PhoneNumber number) : base(id)
    {
        FullName = fullName;
        GeneralDescription = generalDescription;
        AgeExperience = ageExperience;
        PhoneNumber = number;
    }
    public FullName FullName { get; }
    public Description GeneralDescription { get; }
    public AgeExperience AgeExperience { get; }
    public PhoneNumber PhoneNumber { get; }
    public List<Pet> Pets { get; }
    public VolunteerDetails Details { get; }

    public static Result<Volunteer> Create(
        VolunteerId id,
        FullName fullName,
        Description generalDescription,
        AgeExperience ageExperience,
        PhoneNumber number)
    {
        return Result<Volunteer>.Success(
            new Volunteer(id, fullName, generalDescription, 
                ageExperience, number)
            );
    }
    public int PetsAdoptedCount() =>
       Pets.Select(x => x.HelpStatus == HelpStatusPet.FoundHome).Count();
    
    public int PetsFoundHomeQuantity() =>
     Pets.Select(x => x.HelpStatus == HelpStatusPet.LookingForHome).Count();
    
    public int PetsUnderTreatmentCount() => 
         Pets.Select(x => x.HelpStatus == HelpStatusPet.NeedsHelp).Count();
    
}
