using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.VolunteerManagement.ValueObjects;

namespace PetFamily.Application.Features.Volunteers.UpdateRequisites;

public class UpdateRequisitesHandler(
    IVolunteersRepository volunteersRepository,
    IValidator<UpdateRequisitesCommand> validator,
    ILogger<UpdateRequisitesHandler> logger)
{
    public async Task<Result<Guid, ErrorList>> Execute(UpdateRequisitesCommand request,
        CancellationToken token = default)
    {
        var validationResult = await validator.ValidateAsync(request, token);
        if (!validationResult.IsValid)
            return validationResult.ToList();
        
        var volunteerId = VolunteerId.Create(request.Id);
        var volunteer = await volunteersRepository.GetById(volunteerId, token);
        if (volunteer.IsFailure)
            return volunteer.Error.ToErrorList();

        var requisites = request.Requisites
            .Select(x => Requisite.Create(x.Name, x.Description))
            .Select(x => x.Value);

        volunteer.Value.UpdateRequisites(new ValueObjectList<Requisite>(requisites));

        var resultUpdate = await volunteersRepository.Save(volunteer.Value, token);
        if (resultUpdate.IsFailure)
            return resultUpdate.Error.ToErrorList();

        logger.Log(LogLevel.Information, "Volunteer {volunteerId} was updated requisites", volunteerId);
        return resultUpdate.Value;
    }
}