using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Features.VolunteerManagement.Commands.AddFilesPet;

public record AddPhotosToPetCommand(Guid VolunteerId, Guid PetId, IEnumerable<CreateFileCommand> Files) : ICommand;

public record CreateFileCommand(Stream Content, string FileName) : ICommand;