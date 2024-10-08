using PetFamily.Core.Dto;
using PetFamily.VolunteerManagement.Application.Commands.CreateVolunteer;

namespace PetFamily.VolunteerManagement.Presentation.Volunteers;

public record CreateVolunteerRequest(
    FullNameDto FullName,
    string Description,
    int AgeExperience,
    string Number,
    IEnumerable<SocialLinkDto> SocialLinks,
    IEnumerable<RequisiteDto> Requisites)
{
    public CreateVolunteerCommand ToCommand() =>
        new(
            FullName,
            Description,
            AgeExperience,
            Number,
            SocialLinks,
            Requisites);
}