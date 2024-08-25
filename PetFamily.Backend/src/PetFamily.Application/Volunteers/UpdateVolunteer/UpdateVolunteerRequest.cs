using PetFamily.Application.Dto;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Volunteers.UpdateVolunteer;

public record UpdateVolunteerRequest(
    Guid IdVolunteer,
    FullNameDto FullName,
    string Description,
    int AgeExperience,
    string PhoneNumber);
    