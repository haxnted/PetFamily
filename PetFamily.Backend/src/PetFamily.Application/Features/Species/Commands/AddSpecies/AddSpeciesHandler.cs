using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.VolunteerManagement.ValueObjects;

namespace PetFamily.Application.Features.Species.Commands.AddSpecies;

public class AddSpeciesHandler(
    ISpeciesRepository speciesRepository,
    ILogger<AddSpeciesHandler> logger,
    IValidator<AddSpeciesCommand> validator) : ICommandHandler<Guid, AddSpeciesCommand>
{
    public async Task<Result<Guid, ErrorList>> Execute(AddSpeciesCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToList();

        var speciesId = SpeciesId.NewId();
        var typeAnimal = TypeAnimal.Create(command.TypeAnimal).Value;
        var isSpeciesExists = await speciesRepository.GetSpeciesByName(typeAnimal, cancellationToken);
        if (isSpeciesExists.IsSuccess)
            return Errors.Model.AlreadyExist("species").ToErrorList();


        var species = new Domain.Species.Species(speciesId, typeAnimal, []);

        var isSpeciesAdded = await speciesRepository.Add(species, cancellationToken);
        if (isSpeciesAdded.IsFailure)
            return isSpeciesExists.Error.ToErrorList();

        logger.Log(LogLevel.Information, "Species added successfully {Species}.", species);

        return speciesId.Id;
    }
}