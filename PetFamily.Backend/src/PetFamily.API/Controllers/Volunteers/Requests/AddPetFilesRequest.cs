using PetFamily.Application.Features.VolunteerManagement.Commands.AddFilesPet;

namespace PetFamily.API.Controllers.Volunteers;

public record AddPetFilesRequest(IFormFileCollection Files)
{
    public AddPhotosToPetCommand ToCommand(Guid VolunteerId,Guid PetId, IEnumerable<CreateFileCommand> fileContents) =>
        new(VolunteerId, PetId, fileContents);
}