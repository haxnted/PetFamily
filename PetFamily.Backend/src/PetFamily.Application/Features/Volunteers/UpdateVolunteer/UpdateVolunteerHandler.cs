using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Features.Volunteers.UpdateVolunteer;

public class UpdateVolunteerHandler(
    IVolunteersRepository volunteersRepository, 
    IValidator<UpdateVolunteerCommand> validator,
    ILogger<UpdateVolunteerHandler> logger)
{
    public async Task<Result<Guid, ErrorList>> Execute(UpdateVolunteerCommand command, CancellationToken token = default)
    {
        var validationResult = await validator.ValidateAsync(command, token);
        if (!validationResult.IsValid)
            return validationResult.ToList();
        
        var volunteerId = VolunteerId.Create(command.IdVolunteer);
        var volunteer = await volunteersRepository.GetById(volunteerId, token);
        if (volunteer.IsFailure)
            return volunteer.Error.ToErrorList();

        var fullName = FullName.Create(
                command.FullName.Name, 
                command.FullName.Surname, 
                command.FullName.Patronymic).Value;
        var description = Description.Create(command.Description).Value;
        var ageExperience = AgeExperience.Create(command.AgeExperience).Value;
        var phoneNumber = PhoneNumber.Create(command.PhoneNumber).Value;

        volunteer.Value.UpdateMainInfo(fullName, description, ageExperience, phoneNumber);
        
        var resultUpdate = await volunteersRepository.Save(volunteer.Value, token);
        if (resultUpdate.IsFailure) 
            return resultUpdate.Error.ToErrorList();
        
        logger.LogDebug("Volunteer {volunteerId} was full updated", volunteerId);

        return resultUpdate.Value;
    }
}