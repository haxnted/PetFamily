using PetFamily.Application.Features.VolunteerManagement.Commands.RemoveSoftPetById;

namespace PetFamily.API.Controllers.Volunteers;

public record RemoveSoftPetByIdRequest(Guid PetId)
{
    public RemoveSoftPetByIdCommand ToCommand(Guid volunteerId) => 
        new(volunteerId, PetId);
}