using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.Features.Species.Commands.AddSpecies;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;

namespace PetFamily.Application.Features.Species.Commands.DeleteSpecies;

public class DeleteSpeciesHandler(
    IUnitOfWork unitOfWork,
    IReadDbContext readDbContext,
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

        var isSpeciesUsed = readDbContext.Species.FirstOrDefault(s => s.Id == command.SpeciesId);
        if (isSpeciesUsed is not null)
            return Errors.General.AlreadyUsed(command.SpeciesId).ToErrorList();
            
        var speciesId = SpeciesId.Create(command.SpeciesId);
        var species = await speciesRepository.GetSpeciesById(speciesId, cancellationToken);
        if (species.IsFailure)
            return species.Error.ToErrorList();

        speciesRepository.Delete(species.Value, cancellationToken);
        await unitOfWork.SaveChanges(cancellationToken);

        logger.Log(LogLevel.Information, "Species deleted successfully {Species}.", species.Value);

        return speciesId.Id;
    }
}