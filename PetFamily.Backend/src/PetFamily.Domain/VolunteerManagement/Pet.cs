using CSharpFunctionalExtensions;
using PetFamily.Domain.Interfaces;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Domain.VolunteerManagement;

public class Pet : Shared.Entity<PetId>, ISoftDeletable
{
    protected Pet(PetId id) : base(id) { }

    private bool _isDeleted = false;
    
    public PetPhotoList PetPhotoList { get; private set; }
    public RequisiteList RequisiteList { get; private set; }
    public NickName NickName { get; } = null!;
    public Description GeneralDescription { get; } = null!;
    public Description HealthInformation { get; } = null!;
    public BreedId BreedId { get; } = null!;
    public Guid SpeciesId { get; } = Guid.Empty;
    public Address Address { get; }
    public PetPhysicalAttributes PhysicalAttributes { get; } = null!;
    public PhoneNumber PhoneNumber { get; } = null!;
    public DateTime BirthDate { get; }
    public bool IsCastrated { get; }
    public bool IsVaccinated { get; }
    public HelpStatusPet HelpStatus { get; }
    public DateTime DateCreated { get; }

    public Pet(PetId id,
        NickName nickName,
        Description generalDescription,
        Description healthInformation,
        Address address,
        PetPhysicalAttributes attributes,
        Guid speciesId, 
        BreedId breedId,
        PhoneNumber number,
        DateTime birthDate,
        bool isCastrated,
        bool isVaccinated,
        HelpStatusPet helpStatus,
        DateTime dateTime,
        PetPhotoList petPhotoList,
        RequisiteList requisiteList) : base(id)
    {
        PetPhotoList = petPhotoList;
        RequisiteList = requisiteList;
        NickName = nickName;
        GeneralDescription = generalDescription;
        HealthInformation = healthInformation;
        Address = address;
        PhysicalAttributes = attributes;
        BreedId = breedId;
        SpeciesId = speciesId;
        PhoneNumber = number;
        BirthDate = birthDate;
        IsCastrated = isCastrated;
        IsVaccinated = isVaccinated;
        HelpStatus = helpStatus;
        DateCreated = dateTime;
    }

    public UnitResult<Error> UpdateFiles(PetPhotoList list)
    {
        PetPhotoList = list;
        return Result.Success<Error>();
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