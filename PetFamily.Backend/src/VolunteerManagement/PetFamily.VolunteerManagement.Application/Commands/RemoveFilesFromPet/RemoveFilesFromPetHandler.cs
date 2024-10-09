using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Core.Providers.FileProvider;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.EntityIds;
using PetFamily.VolunteerManagement.Application.Commands.AddFilesPet;

namespace PetFamily.VolunteerManagement.Application.Commands.RemoveFilesFromPet;

public class RemoveFilesFromPetHandler(
    IVolunteerUnitOfWork unitOfWork,
    IValidator<RemoveFilesFromPetCommand> validator,
    IVolunteersRepository volunteersRepository,
    IFileProvider fileProvider,
    ILogger<AddPhotosToPetHandler> logger)
    : ICommandHandler<Guid, RemoveFilesFromPetCommand>
{
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
                var filePathPhoto = await fileProvider.GetFileByName(petPhoto.Path,
                    Constants.BUCKET_NAME_FOR_PET_IMAGES, cancellationToken);
                if (filePathPhoto.IsSuccess)
                    await fileProvider.Delete(petPhoto.Path,
                        Constants.BUCKET_NAME_FOR_PET_IMAGES,
                        cancellationToken);
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