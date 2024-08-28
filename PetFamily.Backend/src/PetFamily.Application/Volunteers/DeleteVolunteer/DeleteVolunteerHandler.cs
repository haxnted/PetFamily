using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;

namespace PetFamily.Application.Volunteers.DeleteVolunteer;

public class DeleteVolunteerHandler(IVolunteersRepository repository, ILogger<DeleteVolunteerHandler> logger)
{
    public async Task<Result<Guid, Error>> Execute(DeleteVolunteerRequest request,
        CancellationToken cancellationToken = default)
    {
        var volunteerId = VolunteerId.Create(request.Id);
        var volunteer = await repository.GetById(volunteerId, cancellationToken);
        if (volunteer.IsFailure)
            return volunteer.Error;

        volunteer.Value.Deactivate();
        
        var result = await repository.Delete(volunteer.Value, cancellationToken);
        if (result.IsFailure)
            return volunteer.Error;
        
        logger.Log(LogLevel.Information, "Volunteer deleted with Id {volunteerId}", volunteerId);
        
        return volunteer.Value.Id.Id;
    }
}