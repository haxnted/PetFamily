using PetFamily.Application.Features.VolunteerManagement.Commands.RemoveFilesFromPet;

namespace PetFamily.API.Controllers.Volunteers;

public record RemoveFilesFromPetRequest(Guid PetId)
{
    public RemoveFilesFromPetCommand ToCommand(Guid VolunteerId) =>
        new(VolunteerId, PetId);
}