using PetFamily.Application.Features.VolunteerManagement.Commands.RemoveHardPetById;

namespace PetFamily.API.Controllers.Volunteers;

public record RemoveHardPetByIdRequest(Guid PetId)
{
    public RemoveHardPetByIdCommand ToCommand(Guid volunteerId) => 
        new(volunteerId, PetId);
}