using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Features.VolunteerManagement.Commands.RemoveFilesFromPet;

public record RemoveFilesFromPetCommand(Guid VolunteerId, Guid PetId) : ICommand;