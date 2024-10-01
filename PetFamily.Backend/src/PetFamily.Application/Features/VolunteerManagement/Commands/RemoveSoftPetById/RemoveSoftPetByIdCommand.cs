using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Features.VolunteerManagement.Commands.RemoveSoftPetById;

public record RemoveSoftPetByIdCommand(Guid VolunteerId, Guid PetId) : ICommand;