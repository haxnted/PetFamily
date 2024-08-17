using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models;

public class Pet : Shared.Entity<PetId>
{
    protected Pet(PetId id): base(id) { }
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
    public Requisite Requisite { get; } = null!;
    public DateTimeOffset DateCreated { get; }
    public PetDetails Details { get; } = null!;

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
        Requisite = requisite;
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
            return Errors.General.ValueIsInvalid($"Nickname pet cannot be null or more then {Constants.MIN_TEXT_LENGTH}. ");

        var pet = new Pet(id, nickName, generalDescription, address, attributes, number,
            birthDate, isCastrated, isVaccinated, helpStatus, requisite, dateTimeOffset, details);

        return pet;
    }
}

