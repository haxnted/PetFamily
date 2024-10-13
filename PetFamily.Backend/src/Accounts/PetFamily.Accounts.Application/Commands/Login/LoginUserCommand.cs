using PetFamily.Core.Abstractions;

namespace PetFamily.Accounts.Application.Commands.Login;

public record LoginUserCommand(string Email, string Password) : ICommand;
