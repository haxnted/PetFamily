using PetFamily.Application.Dto;
using PetFamily.Application.Features.Volunteers.AddPet;
using PetFamily.Domain.VolunteerManagement.Enums;

namespace PetFamily.API.Controllers.Volunteers.Requests;

public record AddPetRequest(
    string NickName,
    string GeneralDescription,
    string HealthDescription,
    AddressDto Address,
    double Weight,
    double Height,
    string PhoneNumber,
    DateTime BirthDate,
    bool IsCastrated,
    bool IsVaccinated,
    HelpStatusPet HelpStatus,
    IEnumerable<RequisiteDto> Requisites)
{
    public AddPetCommand ToCommand(Guid Volunteer)
    {
        return new AddPetCommand(Volunteer, 
            NickName, 
            GeneralDescription, 
            HealthDescription, 
            Address, 
            Weight, 
            Height,
            PhoneNumber, 
            BirthDate, 
            IsCastrated, 
            IsVaccinated, 
            HelpStatus, 
            Requisites);
    }
}