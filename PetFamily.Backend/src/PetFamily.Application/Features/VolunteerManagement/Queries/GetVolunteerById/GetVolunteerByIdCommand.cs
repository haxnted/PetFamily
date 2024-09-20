using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Features.VolunteerManagement.Queries.GetVolunteerById;

public record GetVolunteerByIdCommand(Guid Id) : ICommand;