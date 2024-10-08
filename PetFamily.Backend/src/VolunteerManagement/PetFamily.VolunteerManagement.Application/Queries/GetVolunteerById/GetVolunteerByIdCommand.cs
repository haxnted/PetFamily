
using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteerManagement.Application.Queries.GetVolunteerById;

public record GetVolunteerByIdCommand(Guid Id) : ICommand;