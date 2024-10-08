using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteerManagement.Application.Commands.AddFilesPet;

public record AddPhotosToPetCommand(Guid VolunteerId, Guid PetId, IEnumerable<CreateFileCommand> Files) : ICommand;