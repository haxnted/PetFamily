using CSharpFunctionalExtensions;
using PetFamily.Domain.Interfaces;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.VolunteerManagement.Entities;
using PetFamily.Domain.VolunteerManagement.Enums;
using PetFamily.Domain.VolunteerManagement.ValueObjects;

namespace PetFamily.Domain.VolunteerManagement;

public class Volunteer : Shared.Entity<VolunteerId>, ISoftDeletable
{
    private Volunteer(VolunteerId id) : base(id) { }

    public Volunteer(
        VolunteerId id,
        FullName fullName,
        Description generalDescription,
        AgeExperience ageExperience,
        PhoneNumber number,
        ValueObjectList<SocialLink> socialLinkList,
        ValueObjectList<Requisite> requisiteList) : base(id)
    {
        FullName = fullName;
        GeneralDescription = generalDescription;
        AgeExperience = ageExperience;
        PhoneNumber = number;
        SocialLinkList = socialLinkList;
        RequisiteList = requisiteList;
    }

    private bool _isDeleted = false;
    private List<Pet> _pets = [];

    public FullName FullName { get; private set; }
    public Description GeneralDescription { get; private set; }
    public AgeExperience AgeExperience { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public IReadOnlyList<Pet> Pets => _pets;
    public ValueObjectList<SocialLink> SocialLinkList { get; private set; }
    public ValueObjectList<Requisite> RequisiteList { get; private set; }

    public void UpdateSocialLinks(ValueObjectList<SocialLink> list) => 
        SocialLinkList = list;

    public void UpdateRequisites(ValueObjectList<Requisite> list) =>
        RequisiteList = list;

    public UnitResult<Error> AddPet(Pet pet)
    {
        _pets.Add(pet);
        return Result.Success<Error>();
    }
    public void Activate()
    {
        _isDeleted = false;
        foreach (var pet in _pets)

            pet.Activate();
    }
    
    public void Deactivate()
    {
        _isDeleted = true;

        foreach (var pet in _pets)
            pet.Deactivate();
    }

    public Pet? GetPetById(PetId petId) =>
         _pets.FirstOrDefault(x => x.Id == petId);
    
    public void UpdateMainInfo(FullName fullName,
        Description generalDescription,
        AgeExperience ageExperience,
        PhoneNumber number)
    {
        FullName = fullName;
        GeneralDescription = generalDescription;
        AgeExperience = ageExperience;
        PhoneNumber = number;
    }

    public int PetsAdoptedCount() =>
        _pets.Count(x => x.HelpStatus == HelpStatusPet.FoundHome);

    public int PetsFoundHomeQuantity() =>
        _pets.Count(x => x.HelpStatus == HelpStatusPet.LookingForHome);

    public int PetsUnderTreatmentCount() =>
        _pets.Count(x => x.HelpStatus == HelpStatusPet.NeedsHelp);
}