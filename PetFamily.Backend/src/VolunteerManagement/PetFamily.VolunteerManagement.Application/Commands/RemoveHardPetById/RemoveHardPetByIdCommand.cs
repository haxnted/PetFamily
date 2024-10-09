using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteerManagement.Application.Commands.RemoveHardPetById;

public record RemoveHardPetByIdCommand(Guid VolunteerId, Guid PetId) : ICommand;