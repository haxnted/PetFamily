using PetFamily.Core.Abstractions;
using PetFamily.Core.Dto;
using PetFamily.VolunteerManagement.Domain.Enums;

namespace PetFamily.VolunteerManagement.Application.Commands.AddPet;

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
    Guid SpeciesId,
    Guid BreedId,
    bool IsCastrated,
    bool IsVaccinated,
    HelpStatusPet HelpStatus,
    IEnumerable<RequisiteDto> Requisites) : ICommand;