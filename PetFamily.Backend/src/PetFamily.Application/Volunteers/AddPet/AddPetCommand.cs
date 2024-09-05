using PetFamily.Application.Dto;
using PetFamily.Application.FileProvider;
using PetFamily.Domain.VolunteerManagement;

namespace PetFamily.Application.Volunteers.AddPet;

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