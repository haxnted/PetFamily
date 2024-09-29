using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.Features.VolunteerManagement.Commands.AddFilesPet;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Messaging;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.VolunteerManagement.ValueObjects;

namespace PetFamily.Application.Features.VolunteerManagement.Commands.RemoveFilesFromPet;

public class RemoveFilesFromPetHandler(
    IUnitOfWork unitOfWork,
    IValidator<RemoveFilesFromPetCommand> validator,
    IVolunteersRepository volunteersRepository,
    IFileProvider fileProvider,
    ILogger<AddPhotosToPetHandler> logger)
    : ICommandHandler<Guid, RemoveFilesFromPetCommand>
{
    private const string BUCKET_NAME = "files";

    public async Task<Result<Guid, ErrorList>> Execute(RemoveFilesFromPetCommand command,
        CancellationToken cancellationToken = default)
    {
        var validateResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validateResult.IsValid)
            return validateResult.ToList();

        var volunteerId = VolunteerId.Create(command.VolunteerId);
        var volunteer = await volunteersRepository.GetById(volunteerId, cancellationToken);
        if (volunteer.IsFailure)
            return volunteer.Error.ToErrorList();

        var transaction = await unitOfWork.BeginTransaction(cancellationToken);

        try
        {
            var petId = PetId.Create(command.PetId);
            var pet = volunteer.Value.Pets.FirstOrDefault(v => v.Id == petId);
            if (pet is null)
                return Errors.General.NotFound(petId).ToErrorList();

            if (pet.PetPhotoList.Count == 0)
                return command.PetId;

            var petPhotos = pet.PetPhotoList.ToList();
            pet.ClearPhotos();
            await unitOfWork.SaveChanges(cancellationToken);

            foreach (var petPhoto in petPhotos)
            {
                var filePathPhoto = await fileProvider.GetFileByName(petPhoto.Path, BUCKET_NAME, cancellationToken);
                if (filePathPhoto.IsSuccess)
                    await fileProvider.Delete(petPhoto.Path, BUCKET_NAME, cancellationToken);
            }
            
            transaction.Commit();
            logger.Log(LogLevel.Information, "Successful remove medias from pet. Files: {files}", petPhotos);

            return command.PetId;
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            logger.Log(LogLevel.Critical, "Failed remove files from pet. Exception: {ex}", ex);
            return Error.Failure("remove.pet.files", "Failed remove media from pet").ToErrorList();
        }
    }
}