using PetFamily.Application.Features.Volunteers.AddFilesPet;
using PetFamily.Application.FileProvider;

namespace PetFamily.API.Controllers.Volunteers.Requests;

public record AddPetFilesRequest(Guid PetId, IFormFileCollection Files)
{
    public AddPhotosToPetCommand ToCommand(Guid VolunteerId, IEnumerable<CreateFileCommand> fileContents) =>
        new(VolunteerId, PetId, fileContents);
}