using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;

namespace PetFamily.Application.Features.Species.Commands.DeleteBreed;

public class DeleteBreedHandler(
    IUnitOfWork unitOfWork,
    IReadDbContext readDbContext,
    ISpeciesRepository speciesRepository,
    ILogger<DeleteBreedHandler> logger,
    IValidator<DeleteBreedCommand> validator) : ICommandHandler<Guid, DeleteBreedCommand>
{
    public async Task<Result<Guid, ErrorList>> Execute(DeleteBreedCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToList();

        var speciesId = SpeciesId.Create(command.SpeciesId);
        var breedId = BreedId.Create(command.BreedId);

        var isBreedUsed = readDbContext.Pets.FirstOrDefault(p => p.BreedId == breedId.Id);
        if (isBreedUsed is not null)
            return Errors.General.AlreadyUsed(breedId.Id).ToErrorList();

        var species = await speciesRepository.GetSpeciesById(speciesId, cancellationToken);
        if (species.IsFailure)
            return species.Error.ToErrorList();

        var breed = species.Value.Breeds.FirstOrDefault(b => b.Id == breedId);
        if (breed is null)
            return Errors.General.NotFound(command.BreedId).ToErrorList();

        species.Value.RemoveBreed(breed);

        await speciesRepository.Save(species.Value, cancellationToken);
        await unitOfWork.SaveChanges(cancellationToken);

        logger.Log(LogLevel.Information, "Breed deleted successfully {Breed}.", breed);

        return speciesId.Id;
    }
}