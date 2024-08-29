using PetFamily.Domain.Interfaces;
using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Domain.VolunteerManagement;

public class Pet : Shared.Entity<PetId>, ISoftDeletable
{
    protected Pet(PetId id) : base(id) { }

    private bool _isDeleted = false;
    
    public PetPhotoList PetPhotoList { get; }
    public RequisiteList RequisiteList { get; }
    public NickName NickName { get; } = null!;
    public Description GeneralDescription { get; } = null!;
    public Description HealthInformation { get; } = null!;
    public BreedId BreedId { get; } = null!;
    public Guid SpeciesId { get; } 
    public Address Address { get; }
    public PetPhysicalAttributes PhysicalAttributes { get; } = null!;
    public PhoneNumber PhoneNumber { get; } = null!;
    public DateOnly BirthDate { get; }
    public bool IsCastrated { get; }
    public bool IsVaccinated { get; }
    public HelpStatusPet HelpStatus { get; }
    public DateTimeOffset DateCreated { get; }

    public Pet(PetId id,
        NickName nickName,
        Description generalDescription,
        Address address,
        PetPhysicalAttributes attributes,
        PhoneNumber number,
        DateOnly birthDate,
        bool isCastrated,
        bool isVaccinated,
        HelpStatusPet helpStatus,
        DateTimeOffset dateTimeOffset,
        PetPhotoList petPhotoList,
        RequisiteList requisiteList) : base(id)
    {
        PetPhotoList = petPhotoList;
        RequisiteList = requisiteList;
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