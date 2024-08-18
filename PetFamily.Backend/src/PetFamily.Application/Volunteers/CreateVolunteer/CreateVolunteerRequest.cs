using PetFamily.Domain.Models;

namespace PetFamily.Application.Volunteers.CreateVolunteer;

public record CreateVolunteerRequest(
    string Name,
    string Surname,
    string Patronymic,
    string Description,
    int AgeExperience,
    string Number,
    List<SocialLinkDto> SocialLinks,
    List<RequisiteDto> Requisites);

public record SocialLinkDto(string Name, string Url);
public record RequisiteDto(string Name, string Description);
    
     