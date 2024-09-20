using PetFamily.Application.Abstractions;
using PetFamily.Application.Dto;

namespace PetFamily.Application.Features.VolunteerManagement.Commands.UpdateSocialLinks;

public record UpdateSocialLinksCommand(Guid Id, IEnumerable<SocialLinkDto> SocialLinks) : ICommand;