using PetFamily.Application.Dto;
using PetFamily.Domain.VolunteerManagement.Enums;

namespace PetFamily.Application.Features.Volunteers.AddPet;

public record AddPetCommand(
    Guid VolunteerId,
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
    IEnumerable<RequisiteDto> Requisites);