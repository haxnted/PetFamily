using PetFamily.Core.Abstractions;

namespace PetFamily.Accounts.Application.Commands.Register;

public record RegisterUserCommand(string UserName, string Email, string Password) : ICommand;