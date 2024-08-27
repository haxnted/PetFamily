using System.Collections;

namespace PetFamily.Application.Dto;

public record UpdateSocialLinksDto(IEnumerable<SocialLinkDto> SocialLinks);
