namespace PetFamily.Domain.Models;

public class Volunteer
{
    public Guid Id { get; }
    public string Name { get; }
    public string Surname { get; }
    public string Patronymic { get; }
    public string GeneralDescription { get; }
    public string AgeExperience { get; }
    public string PhoneNumber { get; }
    
    public int PetsAdoptedCount { get; }            // Количество которые нашли дом
    public int PetsFoundHomeQuantity  { get; }     // Количество которые ищут дом
    public int PetsUnderTreatmentCount { get; }     // Количество которые находятся на лечении
    
    public List<SocialLink> SocialLinks { get; }
    public List<Requisites> Requisites { get; }
    public List<Pet> Pets { get; }
}