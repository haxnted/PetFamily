using Microsoft.AspNetCore.Http;
using PetFamily.VolunteerManagement.Application.Commands.AddFilesPet;

namespace PetFamily.VolunteerManagement.Presentation.Volunteers.Requests;

public record AddPetFilesRequest(IFormFileCollection Files)
{
    public AddPhotosToPetCommand ToCommand(Guid VolunteerId,Guid PetId, IEnumerable<CreateFileCommand> fileContents) =>
        new(VolunteerId, PetId, fileContents);
}