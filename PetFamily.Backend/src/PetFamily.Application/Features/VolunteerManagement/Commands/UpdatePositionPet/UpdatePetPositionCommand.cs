using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Features.VolunteerManagement.Commands.UpdatePositionPet;

public record UpdatePetPositionCommand(Guid VolunteerId, Guid PetId, int Position) : ICommand;