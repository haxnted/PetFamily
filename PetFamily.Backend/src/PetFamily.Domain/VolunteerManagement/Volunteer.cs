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
    private Volunteer(VolunteerId id) : base(id)
    {
    }

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
    public IReadOnlyList<SocialLink> SocialLinkList { get; private set; }
    public IReadOnlyList<Requisite> RequisiteList { get; private set; }

    public void UpdateSocialLinks(ValueObjectList<SocialLink> list) =>
        SocialLinkList = list;

    public void UpdateRequisites(ValueObjectList<Requisite> list) =>
        RequisiteList = list;

    public UnitResult<Error> AddPet(Pet pet)
    {
        _pets.Add(pet);

        int serialPosition = _pets.Count == 0 ? 1 : _pets.Count;
        pet.ChangePosition(serialPosition);

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
        if (newIdx.Value < oldIdx.Value)
        {
            var collection = _pets
                .Where(p => p.Position.Value >= newIdx.Value && p.Position.Value < oldIdx.Value);
            foreach (var entity in collection)
            {
                entity.ChangePosition(entity.Position.Value + 1);
            }
        }
        else if (newIdx.Value > oldIdx.Value)
        {
            var collection = _pets
                .Where(p => p.Position.Value > oldIdx.Value && p.Position.Value <= newIdx.Value);
            foreach (var entity in collection)
            {
                entity.ChangePosition(entity.Position.Value - 1);
            }
        }
    }

    public int PetsAdoptedCount() =>
        _pets.Count(x => x.HelpStatus == HelpStatusPet.FoundHome);

    public int PetsFoundHomeQuantity() =>
        _pets.Count(x => x.HelpStatus == HelpStatusPet.LookingForHome);

    public int PetsUnderTreatmentCount() =>
        _pets.Count(x => x.HelpStatus == HelpStatusPet.NeedsHelp);
}