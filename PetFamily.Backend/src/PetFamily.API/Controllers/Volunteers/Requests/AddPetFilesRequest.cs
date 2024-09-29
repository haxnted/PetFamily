using PetFamily.Application.Features.VolunteerManagement.Commands.AddFilesPet;

namespace PetFamily.API.Controllers.Volunteers;

public record AddPetFilesRequest(Guid PetId, IFormFileCollection Files)
{
    public AddPhotosToPetCommand ToCommand(Guid VolunteerId, IEnumerable<CreateFileCommand> fileContents) =>
        new(VolunteerId, PetId, fileContents);
}