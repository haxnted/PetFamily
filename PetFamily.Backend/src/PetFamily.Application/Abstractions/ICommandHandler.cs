using CSharpFunctionalExtensions;
using PetFamily.Application.Features.VolunteerManagement.Commands.AddFilesPet;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Abstractions;

public interface ICommandHandler<TResponse, in TCommand> where TCommand : ICommand
{
    public Task<Result<TResponse, ErrorList>> Execute(TCommand command, CancellationToken token = default);
}

public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    public Task<UnitResult<ErrorList>> Execute(TCommand command, CancellationToken token = default);
}