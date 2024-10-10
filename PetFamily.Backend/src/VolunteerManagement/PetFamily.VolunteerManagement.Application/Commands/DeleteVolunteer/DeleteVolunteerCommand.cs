using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteerManagement.Application.Commands.DeleteVolunteer;

public record DeleteVolunteerCommand(Guid Id) : ICommand;