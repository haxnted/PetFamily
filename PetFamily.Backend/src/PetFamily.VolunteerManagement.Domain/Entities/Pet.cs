using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.EntityIds;
using PetFamily.SharedKernel.Interfaces;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.VolunteerManagement.Domain.Enums;
using PetFamily.VolunteerManagement.Domain.ValueObjects;

namespace PetFamily.VolunteerManagement.Domain.Entities;

public class Pet : SharedKernel.Entity<PetId>, ISoftDeletable
{
    protected Pet(PetId id) : base(id) { }

    private bool _isDeleted = false;
    
    public NickName NickName { get; private set; } = null!;
    public Description GeneralDescription { get; private set;} = null!;
    public Description HealthInformation { get; private set;} = null!;
    public BreedId BreedId { get; private set;} = null!;
    public Guid SpeciesId { get; private set;} = Guid.Empty;
    public VolunteerId VolunteerId { get; private set;} = null!;
    public Address Address { get; private set;}
    public PetPhysicalAttributes PhysicalAttributes { get; private set;} = null!;
    public PhoneNumber PhoneNumber { get; private set;} = null!;
    public DateTime BirthDate { get; private set;}
    public bool IsCastrated { get; private set;}
    public bool IsVaccinated { get; private set;}
    public HelpStatusPet HelpStatus { get; private set;}
    public DateTime DateCreated { get; private set;}
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
        List<PetPhoto> petPhotoList,
        List<Requisite> requisiteList) : base(id)
    {
        PetPhotoList = petPhotoList.AsReadOnly();
        RequisiteList = requisiteList.AsReadOnly();
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

    public UnitResult<Error> Update(
        Description generalDescription,
        Description healthInformation,
        Address address,
        PetPhysicalAttributes attributes,
        PhoneNumber number,
        bool isCastrated,
        bool isVaccinated,
        HelpStatusPet helpStatus,
        IReadOnlyList<Requisite> requisiteList
        )
    {
        if (_isDeleted)
            return Error.Failure(
                "failed.update.pet", 
                "Unable to perform surgery because the animal is hidden");

        GeneralDescription = generalDescription;
        HealthInformation = healthInformation;
        Address = address;
        PhysicalAttributes = attributes;
        PhoneNumber = number;
        IsCastrated = isCastrated;
        IsVaccinated = isVaccinated;
        HelpStatus = helpStatus;
        RequisiteList = requisiteList;
        
        return Result.Success<Error>();
    }

    public void ClearPhotos() =>
        PetPhotoList = [];
    
    public void ChangePosition(int position) =>
        Position = position;

    public void UpdateFiles(List<PetPhoto> list) =>
        PetPhotoList = list.AsReadOnly();
    
    public void Activate()
    {
        _isDeleted = false;
    }

    public void Deactivate()
    {
        _isDeleted = true;
    }
}