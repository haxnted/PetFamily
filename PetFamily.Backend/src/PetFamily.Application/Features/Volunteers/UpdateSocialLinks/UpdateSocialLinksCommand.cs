using PetFamily.Application.Dto;

namespace PetFamily.Application.Features.Volunteers.UpdateSocialLinks;

public record UpdateSocialLinksCommand(Guid Id, IEnumerable<SocialLinkDto> SocialLinks);