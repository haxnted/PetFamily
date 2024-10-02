using PetFamily.Application.Features.VolunteerManagement.Commands.UpdatePetPhotoMain;

namespace PetFamily.API.Controllers.Volunteers;

public record UpdatePetPhotoMainRequest(Guid PetId, string FileName)
{
    public UpdatePetPhotoMainCommand ToCommand(Guid VolunteerId) =>
        new(VolunteerId, PetId, FileName);
}