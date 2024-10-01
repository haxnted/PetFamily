using CSharpFunctionalExtensions;
using PetFamily.Domain.Interfaces;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.VolunteerManagement.Enums;
using PetFamily.Domain.VolunteerManagement.ValueObjects;

namespace PetFamily.Domain.VolunteerManagement.Entities;

public class Pet : Shared.Entity<PetId>, ISoftDeletable
{
    protected Pet(PetId id) : base(id) { }

    private bool _isDeleted = false;
    
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
    public Position Position { get; private set; }
    public IReadOnlyList<PetPhoto> PetPhotoList { get; private set; }
    public IReadOnlyList<Requisite> RequisiteList { get; private set; }

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
        ValueObjectList<PetPhoto> petPhotoList,
        ValueObjectList<Requisite> requisiteList) : base(id)
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

    public void ClearPhotos() =>
        PetPhotoList = new ValueObjectList<PetPhoto>([]);
    
    public void ChangePosition(int position) =>
        Position = position;

    public UnitResult<Error> UpdateFiles(ValueObjectList<PetPhoto> list)
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