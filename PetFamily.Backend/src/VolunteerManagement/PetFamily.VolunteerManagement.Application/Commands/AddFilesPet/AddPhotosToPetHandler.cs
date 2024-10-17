using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Core.Messaging;
using PetFamily.Core.Providers.FileProvider;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.EntityIds;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.VolunteerManagement.Domain.ValueObjects;

namespace PetFamily.VolunteerManagement.Application.Commands.AddFilesPet;

public class AddPhotosToPetHandler(
    IVolunteerUnitOfWork unitOfWork,
    IValidator<AddPhotosToPetCommand> validator,
    IVolunteersRepository volunteersRepository,
    IFileProvider fileProvider,
    IMessageQueue<IEnumerable<FilePath>> messageQueue,
    ILogger<AddPhotosToPetHandler> logger) : ICommandHandler<Guid, AddPhotosToPetCommand>
{
    public async Task<Result<Guid, ErrorList>> Execute(AddPhotosToPetCommand command,
        CancellationToken cancellationToken = default)
    {
        var validateResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validateResult.IsValid)
            return validateResult.ToList();

        var volunteerId = VolunteerId.Create(command.VolunteerId);
        var volunteer = await volunteersRepository.GetById(volunteerId, cancellationToken);
        if (volunteer.IsFailure)
            return volunteer.Error.ToErrorList();

        var petId = PetId.Create(command.PetId);
        var pet = volunteer.Value.GetPetById(petId);
        if (pet == null)
            return Errors.General.NotFound(petId).ToErrorList();

        var transaction = await unitOfWork.BeginTransaction(cancellationToken);

        try
        {
            var photosConvert = ConvertToFileContentList(command.Files);
            if (photosConvert.IsFailure)
                return photosConvert.Error.ToErrorList();

            var petPhotoList = photosConvert.Value
                .Select(file => PetPhoto.Create(file.File, false))
                .Select(file => file.Value)
                .ToList();

            var oldPetPhotos = pet.PetPhotoList;
            pet.UpdateFiles(petPhotoList);
            await unitOfWork.SaveChanges(cancellationToken);

            await DeletePetPhotosIfExist(oldPetPhotos, cancellationToken);
            
            var resultUpload = await fileProvider.UploadFiles(photosConvert.Value, cancellationToken);
            if (resultUpload.IsFailure)
            {
                await messageQueue.WriteAsync(photosConvert.Value.Select(p => p.File), cancellationToken);

                return resultUpload.Error.ToErrorList();
            }

            transaction.Commit();

            logger.Log(
                LogLevel.Information,
                "Volunteer {VolunteerId} added photos to pet {PetId}",
                command.VolunteerId,
                command.PetId);

            return command.PetId;
        }
        catch (Exception ex)
        {
            transaction.Rollback();

            logger.Log(
                LogLevel.Information,
                "Transaction failed. Executed command: {pet}, Exception: {Ex}",
                command, ex);
            return Error.Failure("Failed.add.photos", "Failed add photos to pet").ToErrorList();
        }
    }

    public async Task DeletePetPhotosIfExist(IReadOnlyList<PetPhoto> petPhotoList, CancellationToken cancellationToken = default)
    {
        if (petPhotoList.Count == 0)
            return;

        foreach (var petPhoto in petPhotoList)
        {
            var urlPathImage = await fileProvider.GetFileByName(petPhoto.Path, 
                Constants.BUCKET_NAME_FOR_PET_IMAGES, 
                cancellationToken);
            
            if (urlPathImage.IsSuccess)
                await fileProvider.Delete(petPhoto.Path, Constants.BUCKET_NAME_FOR_PET_IMAGES, cancellationToken);
        }
    }

    public Result<IEnumerable<FileContent>, Error> ConvertToFileContentList(IEnumerable<CreateFileCommand> files)
    {
        var fileContentList = new List<FileContent>();
        foreach (var file in files)
        {
            var uniquePath = FilePath.Create(Guid.NewGuid().ToString(), Path.GetExtension(file.FileName));

            if (uniquePath.IsFailure)
                return uniquePath.Error;

            fileContentList.Add(new FileContent(file.Content, uniquePath.Value, Constants.BUCKET_NAME_FOR_PET_IMAGES));
        }

        return fileContentList;
    }
}