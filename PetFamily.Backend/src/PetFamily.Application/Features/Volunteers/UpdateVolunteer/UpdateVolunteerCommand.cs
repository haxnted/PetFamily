using PetFamily.Application.Dto;

namespace PetFamily.Application.Features.Volunteers.UpdateVolunteer;

public record UpdateVolunteerCommand(
    Guid IdVolunteer,
    FullNameDto FullName,
    string Description,
    int AgeExperience,
    string PhoneNumber);
    