using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.EntityIds;
using PetFamily.Species.Application.Commands.AddSpecies;
using PetFamily.VolunteerManagement.Contracts;

namespace PetFamily.Species.Application.Commands.DeleteSpecies;

public class DeleteSpeciesHandler(
    ISpeciesUnitOfWork unitOfWork,
    IVolunteerContract volunteerContract,
    ISpeciesRepository speciesRepository,
    ILogger<AddSpeciesHandler> logger,
    IValidator<DeleteSpeciesCommand> validator) : ICommandHandler<Guid, DeleteSpeciesCommand>
{
    public async Task<Result<Guid, ErrorList>> Execute(
        DeleteSpeciesCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToList();

        var isSpeciesUsed = await volunteerContract.IsPetsUsedSpecies(command.SpeciesId, cancellationToken);
        if (isSpeciesUsed.IsSuccess)
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