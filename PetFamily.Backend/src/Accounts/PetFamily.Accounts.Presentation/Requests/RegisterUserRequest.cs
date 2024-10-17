using PetFamily.Accounts.Application.Commands.Register;

namespace PetFamily.Accounts.Presentation.Requests;

public record RegisterUserRequest(string UserName, string Email, string Password)
{
    public RegisterUserCommand ToCommand() =>
         new (UserName, Email, Password);
    
}
