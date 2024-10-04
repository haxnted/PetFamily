using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.Features.VolunteerManagement.Commands.AddPet;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.VolunteerManagement.ValueObjects;

namespace PetFamily.Application.Features.VolunteerManagement.Commands.UpdateGeneralPetInfo;

public class UpdateGeneralPetInfoHandler(
    IUnitOfWork unitOfWork,
    IVolunteersRepository volunteersRepository,
    IValidator<UpdateGeneralPetInfoCommand> validator,
    ILogger<AddPetHandler> logger) : ICommandHandler<Guid, UpdateGeneralPetInfoCommand>
{
    public async Task<Result<Guid, ErrorList>> Execute(UpdateGeneralPetInfoCommand command, CancellationToken cancellationToken = default)
    {
        var validateResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validateResult.IsValid)
            return validateResult.ToList();
        
        var volunteerId = VolunteerId.Create(command.VolunteerId);
        var volunteer = await volunteersRepository.GetById(volunteerId, cancellationToken);
        if (volunteer.IsFailure)
            return volunteer.Error.ToErrorList();
        
        var petId = PetId.Create(command.PetId);
        var pet = volunteer.Value.Pets.FirstOrDefault(p => p.Id == petId);
        if (pet is null)
            return Errors.General.NotFound(petId).ToErrorList();

        var requisites = command.Requisites
            .Select(r => Requisite.Create(r.Name, r.Description).Value)
            .ToList()
            .AsReadOnly();
        
        var resultUpdate = pet.Update(
            Description.Create(command.GeneralDescription).Value,
            Description.Create(command.HealthDescription).Value,
            Address.Create(command.Address.Street, 
                    command.Address.City, 
                    command.Address.State, 
                    command.Address.ZipCode).Value,
            PetPhysicalAttributes.Create(command.Weight, command.Height).Value,
            PhoneNumber.Create(command.PhoneNumber).Value,
            command.IsCastrated,
            command.IsVaccinated,
            command.HelpStatus,
            requisites
        );
        if (resultUpdate.IsFailure)
            return resultUpdate.Error.ToErrorList();
        
        await volunteersRepository.Save(volunteer.Value, cancellationToken);
        await unitOfWork.SaveChanges(cancellationToken);
        
        logger.Log(LogLevel.Information, "Volunteer {VolunteerId} updated pet {command}", command.VolunteerId, command);
        return pet.Id.Id;
    }

    
}