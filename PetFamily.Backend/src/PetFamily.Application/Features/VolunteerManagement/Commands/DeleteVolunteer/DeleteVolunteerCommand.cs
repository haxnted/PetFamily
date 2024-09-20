using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Features.VolunteerManagement.Commands.DeleteVolunteer;

public record DeleteVolunteerCommand(Guid Id) : ICommand;