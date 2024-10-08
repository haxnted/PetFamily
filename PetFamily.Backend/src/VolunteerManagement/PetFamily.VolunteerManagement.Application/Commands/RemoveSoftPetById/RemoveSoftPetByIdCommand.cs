using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteerManagement.Application.Commands.RemoveSoftPetById;

public record RemoveSoftPetByIdCommand(Guid VolunteerId, Guid PetId) : ICommand;