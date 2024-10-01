using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Features.VolunteerManagement.Commands.RemoveHardPetById;

public record RemoveHardPetByIdCommand(Guid VolunteerId, Guid PetId) : ICommand;