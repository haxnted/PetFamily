using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.EntityIds;

namespace PetFamily.VolunteerManagement.Application.Commands.DeleteVolunteer;

public class DeleteVolunteerHandler(
    IVolunteerUnitOfWork unitOfWork,
    IVolunteersRepository volunteersRepository,
    IValidator<DeleteVolunteerCommand> validator,
    ILogger<DeleteVolunteerHandler> logger) : ICommandHandler<Guid, DeleteVolunteerCommand>
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
        await unitOfWork.SaveChanges(cancellationToken);

        logger.Log(LogLevel.Information, "Volunteer has been deactivated with Id {volunteerId}", volunteerId);

        return volunteer.Value.Id.Id;
    }
}