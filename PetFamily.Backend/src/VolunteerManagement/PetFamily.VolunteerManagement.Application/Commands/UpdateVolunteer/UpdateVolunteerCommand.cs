using PetFamily.Core.Abstractions;
using PetFamily.Core.Dto;

namespace PetFamily.VolunteerManagement.Application.Commands.UpdateVolunteer;

public record UpdateVolunteerCommand(
    Guid IdVolunteer,
    FullNameDto FullName,
    string Description,
    int AgeExperience,
    string PhoneNumber) : ICommand;
    