﻿using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.EntityIds;
using PetFamily.Species.Domain.Entities;


namespace PetFamily.Species.Application.Commands.AddBreedToSpecies;

public class AddBreedToSpeciesHandler(
    ISpeciesRepository speciesRepository,
    ISpeciesUnitOfWork unitOfWork,
    ILogger<AddBreedToSpeciesHandler> logger,
    IValidator<AddBreedToSpeciesCommand> validator) : ICommandHandler<Guid, AddBreedToSpeciesCommand>
{
    public async Task<Result<Guid, ErrorList>> Execute(AddBreedToSpeciesCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToList();
        
        var speciesId = SpeciesId.Create(command.SpeciesId);
        var breedId = BreedId.NewId();
        var breed = Breed.Create(breedId, command.Breed).Value;

        var species = await speciesRepository.GetSpeciesById(speciesId, cancellationToken);
        if (species.IsFailure)
            return species.Error.ToErrorList();

        var isBreedExists = species.Value.Breeds.FirstOrDefault(v => v == breed);
        if (isBreedExists is not null)
            return Errors.Model.AlreadyExist(command.Breed).ToErrorList();

        var resultAddBreed = species.Value.AddBreed(breed);
        if (resultAddBreed.IsFailure)
            return resultAddBreed.Error.ToErrorList();

        speciesRepository.Save(species.Value, cancellationToken);
        await unitOfWork.SaveChanges(cancellationToken);

        logger.Log(LogLevel.Information, "Breed {breed} added successfully to species: {Species}.", breed,
            species.Value);

        return breedId.Id;
    }
}