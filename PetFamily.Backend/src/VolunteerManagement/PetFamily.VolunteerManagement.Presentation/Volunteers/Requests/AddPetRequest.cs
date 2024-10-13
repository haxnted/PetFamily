using PetFamily.Core.Dto;
using PetFamily.VolunteerManagement.Application.Commands.AddPet;
using PetFamily.VolunteerManagement.Domain.Enums;

namespace PetFamily.VolunteerManagement.Presentation.Volunteers.Requests;

public record AddPetRequest(
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
            SpeciesId,
            BreedId,
            IsCastrated,
            IsVaccinated,
            HelpStatus,
            Requisites);
    }
}