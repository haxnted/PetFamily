using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.Features.VolunteerManagement.Commands.RemoveHardPetById;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;

namespace PetFamily.Application.Features.VolunteerManagement.Commands.RemoveSoftPetById;

public class RemoveSoftPetByIdHandler(
    IUnitOfWork unitOfWork,
    IVolunteersRepository volunteersRepository,
    IValidator<RemoveHardPetByIdCommand> validator,
    ILogger<RemoveHardPetByIdCommand> logger
) : ICommandHandler<Guid, RemoveHardPetByIdCommand>

{
    public async Task<Result<Guid, ErrorList>> Execute(RemoveHardPetByIdCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToList();

        var volunteerId = VolunteerId.Create(command.VolunteerId);
        var volunteer = await volunteersRepository.GetById(volunteerId, cancellationToken);
        if (volunteer.IsFailure)
            return volunteer.Error.ToErrorList();

        var petId = PetId.Create(command.PetId);
        var pet = volunteer.Value.Pets.FirstOrDefault(x => x.Id == petId);
        if (pet is null)
            return Errors.General.NotFound(petId).ToErrorList();

        pet.Deactivate();
        await unitOfWork.SaveChanges(cancellationToken);

        logger.Log(LogLevel.Information, "Volunteer {VolunteerId} deactivated pet {PetId}", volunteerId, petId);

        return command.PetId;
    }
}