using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Features.VolunteerManagement.Commands.UpdateVolunteer;

public class UpdateVolunteerHandler(
    IUnitOfWork unitOfWork,
    IVolunteersRepository volunteersRepository, 
    IValidator<UpdateVolunteerCommand> validator,
    ILogger<UpdateVolunteerHandler> logger) : ICommandHandler<Guid, UpdateVolunteerCommand>
{
    public async Task<Result<Guid, ErrorList>> Execute(UpdateVolunteerCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToList();
        
        var volunteerId = VolunteerId.Create(command.IdVolunteer);
        var volunteer = await volunteersRepository.GetById(volunteerId, cancellationToken);
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
        
        await volunteersRepository.Save(volunteer.Value, cancellationToken);
        await unitOfWork.SaveChanges(cancellationToken);
        
        logger.LogDebug("Volunteer {volunteerId} was full updated", volunteerId);

        return volunteer.Value.Id.Id;
    }
}