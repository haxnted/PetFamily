namespace PetFamily.Domain.Models;

public class Volunteer
{
    public Guid Id { get; }
    public string Name { get; }
    public string Surname { get; }
    public string Patronymic { get; }
    public string GeneralDescription { get; }
    public string AgeExperience { get; }
    public string PhoneNumber { get; set; }
    
    public int PetsAdoptedCount { get; set; }            // Количество которые нашли дом
    public int PetsLookingForHomeCount { get; set; }     // Количество которые ищут дом
    public int PetsUnderTreatmentCount { get; set; }     // Количество которые находятся на лечении
    
    public List<SocialLink> SocialLinks { get; }
    public List<Requisites> Requisites { get; }
    public List<Pet> Pets { get; }
}