using PetFamily.Core.Dto;
using PetFamily.VolunteerManagement.Application.Commands.UpdateGeneralPetInfo;
using PetFamily.VolunteerManagement.Domain.Enums;

namespace PetFamily.VolunteerManagement.Presentation.Volunteers;

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