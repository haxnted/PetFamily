using PetFamily.Core.Abstractions;
using PetFamily.Core.Dto;

namespace PetFamily.VolunteerManagement.Application.Commands.CreateVolunteer;

public record CreateVolunteerCommand(
    FullNameDto FullName,
    string Description,
    int AgeExperience,
    string Number,
    IEnumerable<SocialLinkDto> SocialLinks,
    IEnumerable<RequisiteDto> Requisites) : ICommand;