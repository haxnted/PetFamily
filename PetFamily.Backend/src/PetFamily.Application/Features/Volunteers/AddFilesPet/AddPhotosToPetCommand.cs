using PetFamily.Application.FileProvider;

namespace PetFamily.Application.Features.Volunteers.AddFilesPet;

public record AddPhotosToPetCommand(Guid VolunteerId, Guid PetId, IEnumerable<FileContent> Files);