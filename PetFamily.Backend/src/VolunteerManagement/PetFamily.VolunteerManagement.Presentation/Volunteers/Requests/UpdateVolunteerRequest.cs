using PetFamily.Core.Dto;
using PetFamily.VolunteerManagement.Application.Commands.UpdateVolunteer;

namespace PetFamily.VolunteerManagement.Presentation.Volunteers;

public record UpdateVolunteerRequest(
    FullNameDto FullName,
    string Description,
    int AgeExperience,
    string PhoneNumber)
{
    public UpdateVolunteerCommand ToCommand(Guid VolunteerId) => 
        new(VolunteerId, FullName, Description, AgeExperience, PhoneNumber);
}