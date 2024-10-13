using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Application.Commands.Login;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(l => l.Email)
            .NotEmpty().WithError(Errors.General.ValueIsRequired("email"))
            .EmailAddress()
            .WithError(Errors.General.ValueIsInvalid("email"));

        RuleFor(l => l.Password)
            .NotEmpty().WithError(Errors.General.ValueIsRequired("password"))
            .MinimumLength(6)
            .WithError(Errors.General.ValueIsInvalid("password"));
    }
}
