using PetFamily.Application.Abstractions;
using PetFamily.Application.Dto;
using PetFamily.Domain.VolunteerManagement.Enums;

namespace PetFamily.Application.Features.VolunteerManagement.Commands.UpdateGeneralPetInfo;

public record UpdateGeneralPetInfoCommand(
    Guid VolunteerId,
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
    IEnumerable<RequisiteDto> Requisites) : ICommand;