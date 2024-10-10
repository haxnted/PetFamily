using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.EntityIds;
using PetFamily.VolunteerManagement.Domain.ValueObjects;

namespace PetFamily.VolunteerManagement.Application.Commands.UpdatePetPhotoMain;

public class UpdatePetPhotoMainHandler(
    IVolunteerUnitOfWork unitOfWork,
    IVolunteersRepository volunteersRepository,
    IValidator<UpdatePetPhotoMainCommand> validator,
    ILogger<UpdatePetPhotoMainHandler> logger) : ICommandHandler<Guid, UpdatePetPhotoMainCommand>
{
    public async Task<Result<Guid, ErrorList>> Execute(UpdatePetPhotoMainCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToList();

        var volunteerId = VolunteerId.Create(command.VolunteerId);
        var volunteer = await volunteersRepository.GetById(volunteerId, cancellationToken);
        if (volunteer.IsFailure)
            return volunteer.Error.ToErrorList();

        var pet = volunteer.Value.Pets.FirstOrDefault(p => p.Id == command.PetId);
        if (pet is null)
            return Errors.General.NotFound(command.PetId).ToErrorList();

        var pathFile = FilePath.Create(command.FileName).Value;
        var petFile = pet.PetPhotoList.FirstOrDefault(p => p.Path == pathFile);
        if (petFile == null)
            return Errors.General.NotFound().ToErrorList();

        var newPetPhotoList = pet.PetPhotoList.Select(petPhoto => petPhoto.Path != pathFile
                ? PetPhoto.Create(petPhoto.Path).Value
                : PetPhoto.Create(petPhoto.Path, true).Value)
            .ToList();
        
        pet.UpdateFiles([..newPetPhotoList]);

        await volunteersRepository.Save(volunteer.Value, cancellationToken);
        await unitOfWork.SaveChanges(cancellationToken);
        
        logger.Log(LogLevel.Information,
            "Updated PetPhotoLost in Pet {petId}. Current main file - {file}",
            command.PetId,
            command.FileName);

        return command.PetId;
    }
}