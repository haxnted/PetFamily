using PetFamily.Application.Dto;

namespace PetFamily.Application.Volunteers.CreateVolunteer;

public record CreateVolunteerRequest(
    FullNameDto FullName,
    string Description,
    int AgeExperience,
    string Number,
    IEnumerable<SocialLinkDto> SocialLinks,
    IEnumerable<RequisiteDto> Requisites);