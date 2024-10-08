using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteerManagement.Application.Commands.RemoveFilesFromPet;

public record RemoveFilesFromPetCommand(Guid VolunteerId, Guid PetId) : ICommand;