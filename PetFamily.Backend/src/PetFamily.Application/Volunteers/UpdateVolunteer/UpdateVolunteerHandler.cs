using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Volunteers.UpdateVolunteer;

public class UpdateVolunteerHandler(IVolunteersRepository repository, ILogger<UpdateVolunteerHandler> logger)
{
    public async Task<Result<Guid, Error>> Execute(UpdateVolunteerRequest request, CancellationToken token = default)
    {
        var volunteerId = VolunteerId.Create(request.IdVolunteer);

        var volunteer = await repository.GetById(volunteerId, token);

        if (volunteer.IsFailure)
            return volunteer.Error;

        var fullName = FullName.Create(request.FullName.Name, request.FullName.Surname, request.FullName.Patronymic)
            .Value;
        var description = Description.Create(request.Description).Value;
        var ageExperience = AgeExperience.Create(request.AgeExperience).Value;
        var phoneNumber = PhoneNumber.Create(request.PhoneNumber).Value;

        volunteer.Value.UpdateMainInfo(fullName, description, ageExperience, phoneNumber);
        
        var resultUpdate = await repository.Save(volunteer.Value, token);
        if (resultUpdate.IsFailure) 
            return resultUpdate.Error;
        
        logger.LogDebug("Volunteer {volunteerId} was full updated", volunteerId);

        return resultUpdate.Value;
    }
}