using PetFamily.Application.Abstractions;
using PetFamily.Application.Dto;

namespace PetFamily.Application.Features.VolunteerManagement.Commands.UpdateVolunteer;

public record UpdateVolunteerCommand(
    Guid IdVolunteer,
    FullNameDto FullName,
    string Description,
    int AgeExperience,
    string PhoneNumber) : ICommand;
    