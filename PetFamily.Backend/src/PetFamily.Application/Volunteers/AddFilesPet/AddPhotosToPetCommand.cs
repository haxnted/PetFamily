using PetFamily.Application.FileProvider;

namespace PetFamily.Application.Volunteers.AddFilesPet;

public record AddPhotosToPetCommand(Guid VolunteerId, Guid PetId, IEnumerable<FileContent> Files);