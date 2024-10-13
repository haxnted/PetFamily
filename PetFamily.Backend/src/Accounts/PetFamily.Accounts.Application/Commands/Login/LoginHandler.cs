using System.Data;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PetFamily.Accounts.Domain;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;

namespace PetFamily.Accounts.Application.Commands.Login;

public class LoginHandler(
    UserManager<User> userManager,
    ITokenProvider tokenProvider,
    IValidator<LoginUserCommand> validator,
    ILogger<LoginHandler> logger) : ICommandHandler<string, LoginUserCommand>
{
    public async Task<Result<string, ErrorList>> Execute(
        LoginUserCommand command, CancellationToken cancellationToken = default)
    {
        
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToList();
        
        var existsUser = await userManager.FindByEmailAsync(command.Email);
        if (existsUser is null)
            return Errors.User.InvalidCredentials().ToErrorList();

        var passwordCorrect = await userManager.CheckPasswordAsync(existsUser, command.Password);
        if (!passwordCorrect)
            return Errors.User.InvalidCredentials().ToErrorList();

        var token = tokenProvider.GenerateAccessToken(existsUser, cancellationToken);
        return token;
    }
}