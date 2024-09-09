using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;

namespace PetFamily.Application.Features.Volunteers.UpdatePositionPet;

public class UpdatePetPositionHandler(
    IVolunteersRepository volunteersRepository,
    IValidator<UpdatePetPositionCommand> validator,
    ILogger<UpdatePetPositionHandler> logger)
{
    public async Task<Result<Guid, ErrorList>> Execute(
        UpdatePetPositionCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            validationResult.ToList();

        var volunteerId = VolunteerId.Create(command.VolunteerId);
        var volunteer = await volunteersRepository.GetById(volunteerId, cancellationToken);
        if (volunteer.IsFailure)
            return volunteer.Error.ToErrorList();

        var petId = PetId.Create(command.PetId);
        var resultPetUpdate = volunteer.Value.MovePet(petId, command.Position);
        if (resultPetUpdate.IsFailure)
            return resultPetUpdate.Error.ToErrorList();

        await volunteersRepository.Save(volunteer.Value, cancellationToken);
        
        logger.LogInformation("Position of pet has been updated. PetId: {petId}. New position: {position}",
            petId, command.Position);

        return petId.Id;
    }
}