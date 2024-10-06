using PetFamily.VolunteerManagement.Application.Commands.UpdatePetPhotoMain;

namespace PetFamily.VolunteerManagement.Presentation.Volunteers.Requests;

public record UpdatePetPhotoMainRequest(Guid PetId, string FileName)
{
    public UpdatePetPhotoMainCommand ToCommand(Guid VolunteerId) =>
        new(VolunteerId, PetId, FileName);
}