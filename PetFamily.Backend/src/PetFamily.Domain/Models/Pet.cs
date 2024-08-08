namespace PetFamily.Domain.Models;

public class Pet
{
    public Guid Id { get; }
    public string NickName { get; }
    public TypeAnimal TypeAnimal { get; } // Тип животного
    public string GeneralDescription { get; } // Общее описание животного
    public string Breed { get; }  // Порода животного
    public string Color { get; } // Окрас животного
    public string PetHealthInformation { get; }  // Информация о здоровье животного
    public string Address { get; } 
    public double Weight { get; } // Вес
    public double Height { get; } // Рост
    public string PhoneNumber { get; } 
    public DateOnly BirthDate { get; }
    public bool IsCastrated { get; } 
    public bool IsVaccinated { get; } 
    public HelpStatusPet HelpStatus { get; }
    public Requisites Requisites { get; }
    public DateTimeOffset DateCreated { get; }
    
    public List<PetPhoto> Photos { get; }
}

