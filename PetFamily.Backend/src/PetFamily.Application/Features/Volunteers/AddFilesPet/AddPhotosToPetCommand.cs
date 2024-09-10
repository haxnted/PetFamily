using PetFamily.Application.FileProvider;

namespace PetFamily.Application.Features.Volunteers.AddFilesPet;

public record AddPhotosToPetCommand(Guid VolunteerId, Guid PetId, IEnumerable<CreateFileCommand> Files);

public record CreateFileCommand(Stream Content, string FileName);