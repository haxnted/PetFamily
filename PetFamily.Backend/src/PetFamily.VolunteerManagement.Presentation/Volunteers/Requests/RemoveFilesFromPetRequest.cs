using PetFamily.VolunteerManagement.Application.Commands.RemoveFilesFromPet;

namespace PetFamily.VolunteerManagement.Presentation.Volunteers.Requests;

public record RemoveFilesFromPetRequest
{
    public RemoveFilesFromPetCommand ToCommand(Guid VolunteerId, Guid PetId) =>
        new(VolunteerId, PetId);
}