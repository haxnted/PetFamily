using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.EntityIds;
using PetFamily.Species.Domain.ValueObjects;
using PetFamily.VolunteerManagement.Domain.ValueObjects;

namespace PetFamily.Species.Application.Commands.AddSpecies;

public class AddSpeciesHandler(
    ISpeciesUnitOfWork unitOfWork,
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


        var species = new Domain.Species(speciesId, typeAnimal, []);

        await speciesRepository.Add(species, cancellationToken);
        await unitOfWork.SaveChanges(cancellationToken);
        
        logger.Log(LogLevel.Information, "Species added successfully {Species}.", species);

        return speciesId.Id;
    }
}