using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models;

public class Volunteer
{
    protected Volunteer() {}
    
    public VolunteerId Id { get; }
    
    public FullName FullName { get; }
    public Description GeneralDescription { get; }
    public AgeExperience AgeExperience { get; }
    public PhoneNumber PhoneNumber { get; }
    
    public int PetsAdoptedCount { get; }            // Количество которые нашли дом
    public int PetsFoundHomeQuantity  { get; }     // Количество которые ищут дом
    public int PetsUnderTreatmentCount { get; }     // Количество которые находятся на лечении
    
    public List<Pet> Pets { get; }
    
    public VolunteerDetails Details { get; }
}