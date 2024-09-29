using PetFamily.Application.Dto;
using PetFamily.Application.Features.VolunteerManagement.Commands.UpdateVolunteer;

namespace PetFamily.API.Controllers.Volunteers;

public record UpdateVolunteerRequest(
    FullNameDto FullName,
    string Description,
    int AgeExperience,
    string PhoneNumber)
{
    public UpdateVolunteerCommand ToCommand(Guid VolunteerId) => 
        new(VolunteerId, FullName, Description, AgeExperience, PhoneNumber);
}