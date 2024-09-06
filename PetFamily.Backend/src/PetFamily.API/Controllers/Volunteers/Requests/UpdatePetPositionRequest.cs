using PetFamily.Application.Features.Volunteers.UpdatePositionPet;

namespace PetFamily.API.Controllers.Volunteers.Requests;

public record UpdatePetPositionRequest(Guid PetId, int position)
{
    public UpdatePetPositionCommand ToCommand(Guid VolunteerId) =>
        new(VolunteerId, PetId, position);
}