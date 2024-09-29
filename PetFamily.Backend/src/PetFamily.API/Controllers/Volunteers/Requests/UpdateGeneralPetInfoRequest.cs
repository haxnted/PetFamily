using PetFamily.Application.Dto;
using PetFamily.Application.Features.VolunteerManagement.Commands.UpdateGeneralPetInfo;
using PetFamily.Domain.VolunteerManagement.Enums;

namespace PetFamily.API.Controllers.Volunteers;

public record UpdateGeneralPetInfoRequest(
    string NickName,
    string GeneralDescription,
    string HealthDescription,
    AddressDto Address,
    double Weight,
    double Height,
    string PhoneNumber,
    DateTime BirthDate,
    Guid SpeciesId,
    Guid BreedId,
    bool IsCastrated,
    bool IsVaccinated,
    HelpStatusPet HelpStatus,
    IEnumerable<RequisiteDto> Requisites)
{
    public UpdateGeneralPetInfoCommand ToCommand(Guid Volunteer)
    {
        return new UpdateGeneralPetInfoCommand(Volunteer,
            NickName,
            GeneralDescription,
            HealthDescription,
            Address,
            Weight,
            Height,
            PhoneNumber,
            BirthDate,
            SpeciesId,
            BreedId,
            IsCastrated,
            IsVaccinated,
            HelpStatus,
            Requisites);
    }
}