using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.VolunteerManagement;

namespace PetFamily.Application.Volunteers.UpdateRequisites;

public class UpdateRequisitesHandler(IVolunteersRepository repository, ILogger<UpdateRequisitesHandler> logger)
{
    public async Task<Result<Guid, Error>> Execute(UpdateRequisitesRequest request,
        CancellationToken token = default)
    {
        var volunteerId = VolunteerId.Create(request.Id);

        var volunteer = await repository.GetById(volunteerId, token);

        if (volunteer.IsFailure)
            return volunteer.Error;

        var requisites = request.Requisites
            .Select(x => Requisite.Create(x.Name, x.Description))
            .Select(x => x.Value);
        var requisitesList = new RequisiteList(requisites);

        volunteer.Value.UpdateRequisites(requisitesList);
        
        var resultUpdate = await repository.Save(volunteer.Value, token);
        if (resultUpdate.IsFailure)
            return resultUpdate.Error;

        logger.Log(LogLevel.Information, "Volunteer {volunteerId} was updated requisites", volunteerId);
        return resultUpdate.Value;
    }
}