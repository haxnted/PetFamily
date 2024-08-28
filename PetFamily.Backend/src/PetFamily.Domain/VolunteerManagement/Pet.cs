using CSharpFunctionalExtensions;
using PetFamily.Domain.Interfaces;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Domain.VolunteerManagement;

public class Pet : Shared.Entity<PetId>, ISoftDeletable
{
    protected Pet(PetId id) : base(id)
    {
    }

    public string NickName { get; } = string.Empty;
    public Description GeneralDescription { get; } = null!;
    public Description HealthInformation { get; } = null!;
    public BreedId BreedId { get; } = null!;
    public SpeciesId SpeciesId { get; } = null!;
    public Address Address { get; } = null!;
    public PetPhysicalAttributes PhysicalAttributes { get; } = null!;
    public PhoneNumber PhoneNumber { get; } = null!;
    public DateOnly BirthDate { get; }
    public bool IsCastrated { get; }
    public bool IsVaccinated { get; }
    public HelpStatusPet HelpStatus { get; }
    public DateTimeOffset DateCreated { get; }
    public PetDetails Details { get; } = null!;
    private bool _isDeleted = false;

    private Pet(PetId id,
        string nickName,
        Description generalDescription,
        Address address,
        PetPhysicalAttributes attributes,
        PhoneNumber number,
        DateOnly birthDate,
        bool isCastrated,
        bool isVaccinated,
        HelpStatusPet helpStatus,
        Requisite requisite,
        DateTimeOffset dateTimeOffset,
        PetDetails details) : base(id)
    {
        NickName = nickName;
        GeneralDescription = generalDescription;
        Address = address;
        PhysicalAttributes = attributes;
        PhoneNumber = number;
        BirthDate = birthDate;
        IsCastrated = isCastrated;
        IsVaccinated = isVaccinated;
        HelpStatus = helpStatus;
        DateCreated = dateTimeOffset;
        Details = details;
    }

    public static Result<Pet, Error> Create(PetId id,
        string nickName,
        Description generalDescription,
        Address address,
        PetPhysicalAttributes attributes,
        PhoneNumber number,
        DateOnly birthDate,
        bool isCastrated,
        bool isVaccinated,
        HelpStatusPet helpStatus,
        Requisite requisite,
        DateTimeOffset dateTimeOffset,
        PetDetails details)
    {
        if (string.IsNullOrEmpty(nickName) || nickName.Length > Constants.MIN_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid("Nickname");

        var pet = new Pet(id, nickName, generalDescription, address, attributes, number,
            birthDate, isCastrated, isVaccinated, helpStatus, requisite, dateTimeOffset, details);

        return pet;
    }

    public void Activate()
    {
        _isDeleted = false;
    }

    public void Deactivate()
    {
        _isDeleted = true;
    }
}