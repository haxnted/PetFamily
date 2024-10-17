using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Application.Commands.Register;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(l => l.Email)
            .NotEmpty().WithError(Errors.General.ValueIsRequired("email"))
            .EmailAddress().WithError(Errors.General.ValueIsInvalid("email"));

        RuleFor(l => l.Password)
            .NotEmpty().WithError(Errors.General.ValueIsRequired("password"))
            .MinimumLength(6).WithError(Errors.General.ValueIsInvalid("password"));

        RuleFor(l => l.UserName)
            .NotEmpty().WithError(Errors.General.ValueIsRequired("username"))
            .MinimumLength(3).WithError(Errors.General.ValueIsInvalid("username"))
            .Matches("^[a-zA-Z0-9]*$").WithError(Errors.General.ValueIsInvalid("username"));

    }
}
