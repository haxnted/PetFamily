using PetFamily.Application.Abstractions;
using PetFamily.Application.Dto;
using PetFamily.Domain.VolunteerManagement.Enums;

namespace PetFamily.Application.Features.VolunteerManagement.Commands.UpdateGeneralPetInfo;

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