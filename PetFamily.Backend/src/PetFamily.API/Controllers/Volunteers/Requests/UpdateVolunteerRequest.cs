using PetFamily.Application.Dto;
using PetFamily.Application.Features.Volunteers.UpdateVolunteer;

namespace PetFamily.API.Controllers.Volunteers.Requests;

public record UpdateVolunteerRequest(
    FullNameDto FullName,
    string Description,
    int AgeExperience,
    string PhoneNumber)
{
    public UpdateVolunteerCommand ToCommand(Guid VolunteerId) => 
        new(VolunteerId, FullName, Description, AgeExperience, PhoneNumber);
}