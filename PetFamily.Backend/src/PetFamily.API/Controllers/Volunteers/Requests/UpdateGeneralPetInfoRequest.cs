using PetFamily.Application.Dto;
using PetFamily.Application.Features.VolunteerManagement.Commands.UpdateGeneralPetInfo;
using PetFamily.Domain.VolunteerManagement.Enums;

namespace PetFamily.API.Controllers.Volunteers;

public record UpdateGeneralPetInfoRequest(
    string GeneralDescription,
    string HealthDescription,
    AddressDto Address,
    double Weight,
    double Height,
    string PhoneNumber,
    bool IsCastrated,
    bool IsVaccinated,
    HelpStatusPet HelpStatus,
    IEnumerable<RequisiteDto> Requisites)
{
    public UpdateGeneralPetInfoCommand ToCommand(Guid volunteerId, Guid petId)
    {
        return new UpdateGeneralPetInfoCommand(volunteerId,
            petId,
            GeneralDescription,
            HealthDescription,
            Address,
            Weight,
            Height,
            PhoneNumber,
            IsCastrated,
            IsVaccinated,
            HelpStatus,
            Requisites);
    }
}