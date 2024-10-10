using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteerManagement.Application.Commands.UpdatePositionPet;

public record UpdatePetPositionCommand(Guid VolunteerId, Guid PetId, int Position) : ICommand;