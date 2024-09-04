using PetFamily.Application.Dto;
using PetFamily.Domain.VolunteerManagement;

namespace PetFamily.API.Contracts;

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
    IEnumerable<RequisiteDto> Requisites);