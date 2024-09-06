using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;

namespace PetFamily.Application.Features.Volunteers.DeleteVolunteer;

public class DeleteVolunteerHandler(
    IVolunteersRepository volunteersRepository,
    IValidator<DeleteVolunteerCommand> validator,
    ILogger<DeleteVolunteerHandler> logger)
{
    public async Task<Result<Guid, ErrorList>> Execute(DeleteVolunteerCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToList();

        var volunteerId = VolunteerId.Create(command.Id);
        var volunteer = await volunteersRepository.GetById(volunteerId, cancellationToken);
        if (volunteer.IsFailure)
            return volunteer.Error.ToErrorList();

        volunteer.Value.Deactivate();

        var result = await volunteersRepository.Delete(volunteer.Value, cancellationToken);
        if (result.IsFailure)
            return volunteer.Error.ToErrorList();

        logger.Log(LogLevel.Information, "Volunteer deleted with Id {volunteerId}", volunteerId);

        return volunteer.Value.Id.Id;
    }
}