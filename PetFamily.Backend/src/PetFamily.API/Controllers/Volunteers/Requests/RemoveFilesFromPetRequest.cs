using PetFamily.Application.Features.VolunteerManagement.Commands.RemoveFilesFromPet;

namespace PetFamily.API.Controllers.Volunteers;

public record RemoveFilesFromPetRequest
{
    public RemoveFilesFromPetCommand ToCommand(Guid VolunteerId, Guid PetId) =>
        new(VolunteerId, PetId);
}