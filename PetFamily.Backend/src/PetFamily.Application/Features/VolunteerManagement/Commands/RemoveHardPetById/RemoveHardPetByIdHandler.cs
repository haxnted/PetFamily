using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Messaging;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.VolunteerManagement.ValueObjects;

namespace PetFamily.Application.Features.VolunteerManagement.Commands.RemoveHardPetById;

public class RemoveHardPetByIdHandler(
    IUnitOfWork unitOfWork,
    IFileProvider fileProvider,
    IVolunteersRepository volunteersRepository,
    IValidator<RemoveHardPetByIdCommand> validator,
    IMessageQueue<IEnumerable<FilePath>> messageQueue,
    ILogger<RemoveHardPetByIdCommand> logger
) : ICommandHandler<Guid, RemoveHardPetByIdCommand>
{
    private const string BUCKET_NAME = "files";

    public async Task<Result<Guid, ErrorList>> Execute(RemoveHardPetByIdCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToList();

        var volunteerId = VolunteerId.Create(command.VolunteerId);
        var volunteer = await volunteersRepository.GetById(volunteerId, cancellationToken);
        if (volunteer.IsFailure)
            return volunteer.Error.ToErrorList();

        var petId = PetId.Create(command.PetId);
        var pet = volunteer.Value.Pets.FirstOrDefault(x => x.Id == petId);
        if (pet is null)
            return Errors.General.ValueIsInvalid().ToErrorList();

        var petMediaPaths = pet.PetPhotoList.Select(p => p.Path);
        volunteer.Value.HardRemovePet(pet);

        await unitOfWork.SaveChanges(cancellationToken);
        
        var filePaths = new List<FilePath>();
        foreach (var path in petMediaPaths)
        {
            var file = await fileProvider.GetFileByName(path, BUCKET_NAME, cancellationToken);
            if (file.IsSuccess)
                filePaths.Add(path);
        }

        await messageQueue.WriteAsync(filePaths, cancellationToken);
    
        logger.Log(LogLevel.Information, "Pet {pet} was removed", pet);

        return command.PetId;
    }
}