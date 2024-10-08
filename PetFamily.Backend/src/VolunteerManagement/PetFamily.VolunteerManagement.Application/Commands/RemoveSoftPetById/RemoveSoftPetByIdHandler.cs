using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.EntityIds;
using PetFamily.VolunteerManagement.Application.Commands.RemoveHardPetById;

namespace PetFamily.VolunteerManagement.Application.Commands.RemoveSoftPetById;

public class RemoveSoftPetByIdHandler(
    IVolunteerUnitOfWork unitOfWork,
    IVolunteersRepository volunteersRepository,
    IValidator<RemoveSoftPetByIdCommand> validator,
    ILogger<RemoveHardPetByIdCommand> logger
) : ICommandHandler<Guid, RemoveSoftPetByIdCommand>

{
    public async Task<Result<Guid, ErrorList>> Execute(RemoveSoftPetByIdCommand command,
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