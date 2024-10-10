using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteerManagement.Application.Commands.AddFilesPet;

public record CreateFileCommand(Stream Content, string FileName) : ICommand;