using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Extensions;
using PetFamily.Application.Features.Species.Commands.AddSpecies;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;

namespace PetFamily.Application.Features.Species.Commands.DeleteSpecies;

public class DeleteSpeciesHandler(
    ISpeciesRepository speciesRepository,
    ILogger<AddSpeciesHandler> logger,
    IValidator<DeleteSpeciesCommand> validator) : ICommandHandler<Guid, DeleteSpeciesCommand>
{
    public async Task<Result<Guid, ErrorList>> Execute(DeleteSpeciesCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToList();

        var speciesId = SpeciesId.Create(command.SpeciesId);
        var species = await speciesRepository.GetSpeciesById(speciesId, cancellationToken);
        if (species.IsFailure)
            return species.Error.ToErrorList();

        await speciesRepository.Delete(species.Value, cancellationToken);
        
        logger.Log(LogLevel.Information, "Species deleted successfully {Species}.", species.Value);

        return speciesId.Id;
    }
}