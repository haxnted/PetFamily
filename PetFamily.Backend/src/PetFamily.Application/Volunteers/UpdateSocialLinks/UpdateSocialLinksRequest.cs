using PetFamily.Application.Dto;

namespace PetFamily.Application.Volunteers.UpdateSocialLinks;

public record UpdateSocialLinksRequest(Guid Id, IEnumerable<SocialLinkDto> SocialLinks);