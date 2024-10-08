using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.EntityIds;
using PetFamily.SharedKernel.Interfaces;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.VolunteerManagement.Domain.Entities;
using PetFamily.VolunteerManagement.Domain.Enums;
using PetFamily.VolunteerManagement.Domain.ValueObjects;

namespace PetFamily.VolunteerManagement.Domain;

public class Volunteer : SharedKernel.Entity<VolunteerId>, ISoftDeletable
{
    private Volunteer(VolunteerId id) : base(id) { }

    public Volunteer(
        VolunteerId id,
        FullName fullName,
        Description generalDescription,
        AgeExperience ageExperience,
        PhoneNumber number,
        List<SocialLink> socialLinkList,
        List<Requisite> requisiteList) : base(id)
    {
        FullName = fullName;
        GeneralDescription = generalDescription;
        AgeExperience = ageExperience;
        PhoneNumber = number;
        SocialLinkList = socialLinkList.AsReadOnly();
        RequisiteList = requisiteList.AsReadOnly();
    }

    private bool _isDeleted = false;
    private List<Pet> _pets = [];

    public FullName FullName { get; private set; }
    public Description GeneralDescription { get; private set; }
    public AgeExperience AgeExperience { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public IReadOnlyList<Pet> Pets => _pets;
    public IReadOnlyList<SocialLink> SocialLinkList { get; private set; }
    public IReadOnlyList<Requisite> RequisiteList { get; private set; }

    public void UpdateSocialLinks(IReadOnlyList<SocialLink> list) =>
        SocialLinkList = list;

    public void UpdateRequisites(IReadOnlyList<Requisite> list) =>
        RequisiteList = list;

    public void AddPet(Pet pet)
    {
        _pets.Add(pet);
        pet.ChangePosition(_pets.Count == 0 ? 1 : _pets.Count);
        
    }
    
    public void HardRemovePet(Pet pet) =>
        _pets.Remove(pet);
    
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

    public UnitResult<Error> MovePet(PetId id, Position newIdx)
    {
        if (_pets.Count == 1)
            return Errors.General.InsufficientItems("pets");

        if (newIdx.Value < 1 || newIdx.Value > Pets.Count)
            return Errors.General.ValueIsInvalid("newIdx");

        var pet = _pets.FirstOrDefault(p => p.Id == id);
        if (pet is null)
            return Errors.General.NotFound(id);

        if (pet.Position == newIdx)
            return Result.Success<Error>();

        var oldIdx = Position.Create(pet.Position.Value);
        UpdatePositions(newIdx, oldIdx.Value);
        pet.ChangePosition(newIdx.Value);

        return Result.Success<Error>();
    }

    private void UpdatePositions(Position newIdx, Position oldIdx)
    {
        var collection = newIdx.Value < oldIdx.Value ?
            _pets.Where(p => p.Position.Value >= newIdx.Value && p.Position.Value < oldIdx.Value):
            _pets.Where(p => p.Position.Value > oldIdx.Value && p.Position.Value <= newIdx.Value);
        
        foreach (var entity in collection)
        {
            entity.ChangePosition(newIdx.Value < oldIdx.Value ? entity.Position.Value + 1 : entity.Position.Value - 1);
        }
    }

    public int PetsAdoptedCount() =>
        _pets.Count(x => x.HelpStatus == HelpStatusPet.FoundHome);

    public int PetsFoundHomeQuantity() =>
        _pets.Count(x => x.HelpStatus == HelpStatusPet.LookingForHome);

    public int PetsUnderTreatmentCount() =>
        _pets.Count(x => x.HelpStatus == HelpStatusPet.NeedsHelp);
}