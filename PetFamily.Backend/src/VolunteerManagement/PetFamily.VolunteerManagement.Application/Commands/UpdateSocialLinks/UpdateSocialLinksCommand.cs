using PetFamily.Core.Abstractions;
using PetFamily.Core.Dto;

namespace PetFamily.VolunteerManagement.Application.Commands.UpdateSocialLinks;

public record UpdateSocialLinksCommand(Guid Id, IEnumerable<SocialLinkDto> SocialLinks) : ICommand;