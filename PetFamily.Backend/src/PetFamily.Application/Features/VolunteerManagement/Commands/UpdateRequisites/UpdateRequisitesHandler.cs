using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.VolunteerManagement.ValueObjects;

namespace PetFamily.Application.Features.VolunteerManagement.Commands.UpdateRequisites;

public class UpdateRequisitesHandler(
    IUnitOfWork unitOfWork,
    IVolunteersRepository volunteersRepository,
    IValidator<UpdateRequisitesCommand> validator,
    ILogger<UpdateRequisitesHandler> logger) : ICommandHandler<Guid, UpdateRequisitesCommand>
{
    public async Task<Result<Guid, ErrorList>> Execute(UpdateRequisitesCommand request,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToList();
        
        var volunteerId = VolunteerId.Create(request.Id);
        var volunteer = await volunteersRepository.GetById(volunteerId, cancellationToken);
        if (volunteer.IsFailure)
            return volunteer.Error.ToErrorList();

        var requisites = request.Requisites
            .Select(x => Requisite.Create(x.Name, x.Description))
            .Select(x => x.Value);

        volunteer.Value.UpdateRequisites(new ValueObjectList<Requisite>(requisites));

        await volunteersRepository.Save(volunteer.Value, cancellationToken);
        await unitOfWork.SaveChanges(cancellationToken);

        logger.Log(LogLevel.Information, "Volunteer {volunteerId} was updated requisites", volunteerId);
        return volunteer.Value.Id.Id;
    }
}