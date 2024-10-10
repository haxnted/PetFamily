using PetFamily.Core.Abstractions;
using PetFamily.Core.Dto;
using PetFamily.VolunteerManagement.Domain.Enums;

namespace PetFamily.VolunteerManagement.Application.Commands.UpdateGeneralPetInfo;

public record UpdateGeneralPetInfoCommand(
    Guid VolunteerId,
    Guid PetId,
    string GeneralDescription,
    string HealthDescription,
    AddressDto Address,
    double Weight,
    double Height,
    string PhoneNumber,
    bool IsCastrated,
    bool IsVaccinated,
    HelpStatusPet HelpStatus,
    IEnumerable<RequisiteDto> Requisites) : ICommand;