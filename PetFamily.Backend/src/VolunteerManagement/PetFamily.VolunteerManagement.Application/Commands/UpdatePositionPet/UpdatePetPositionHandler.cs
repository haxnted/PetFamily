using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.EntityIds;
using PetFamily.VolunteerManagement.Domain.ValueObjects;

namespace PetFamily.VolunteerManagement.Application.Commands.UpdatePositionPet;

public class UpdatePetPositionHandler(
    IVolunteerUnitOfWork unitOfWork,
    IVolunteersRepository volunteersRepository,
    IValidator<UpdatePetPositionCommand> validator,
    ILogger<UpdatePetPositionHandler> logger) : ICommandHandler<Guid, UpdatePetPositionCommand>
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
        var position = Position.Create(command.Position);
        if (position.IsFailure)
            return position.Error.ToErrorList();
        
        var resultPetUpdate = volunteer.Value.MovePet(petId, position.Value);
        if (resultPetUpdate.IsFailure)
            return resultPetUpdate.Error.ToErrorList();

        await volunteersRepository.Save(volunteer.Value, cancellationToken);
        await unitOfWork.SaveChanges(cancellationToken);
        
        logger.LogInformation("Position of pet has been updated. PetId: {petId}. New position: {position}",
            petId, command.Position);

        return petId.Id;
    }
}