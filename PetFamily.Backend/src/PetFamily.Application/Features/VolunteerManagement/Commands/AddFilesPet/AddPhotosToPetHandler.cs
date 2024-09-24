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
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.VolunteerManagement.ValueObjects;

namespace PetFamily.Application.Features.VolunteerManagement.Commands.AddFilesPet;



public class AddPhotosToPetHandler(
    IUnitOfWork unitOfWork,
    IValidator<AddPhotosToPetCommand> validator,
    IVolunteersRepository volunteersRepository,
    IFileProvider fileProvider,
    IMessageQueue<IEnumerable<FilePath>> messageQueue,
    ILogger<AddPhotosToPetHandler> logger) : ICommandHandler<Guid, AddPhotosToPetCommand>
{
    private const string BUCKET_NANE = "files";

    public async Task<Result<Guid, ErrorList>> Execute(AddPhotosToPetCommand command, CancellationToken cancellationToken = default)
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
            var petPhotosConvert = new List<FileContent>();
            foreach (var file in command.Files)
            {
                var extensionFile = Path.GetExtension(file.FileName);
                var uniquePath = FilePath.Create(Guid.NewGuid().ToString(), extensionFile);
                if (uniquePath.IsFailure)
                    return uniquePath.Error.ToErrorList();

                petPhotosConvert.Add(new FileContent(file.Content, uniquePath.Value, BUCKET_NANE));
            }

            var photosConvert = petPhotosConvert.ToList();

            var petPhotoList = photosConvert
                .Select(file => PetPhoto.Create(file.File, false))
                .Select(file => file.Value);

            pet.UpdateFiles(new ValueObjectList<PetPhoto>(petPhotoList));
            await unitOfWork.SaveChanges(cancellationToken);

            var resultUpload = await fileProvider.UploadFiles(photosConvert, cancellationToken);
            if (resultUpload.IsFailure)
            {
                await messageQueue.WriteAsync(photosConvert.Select(p => p.File), cancellationToken);
                
                return resultUpload.Error.ToErrorList();
            }

            transaction.Commit();

            logger.Log(
                LogLevel.Information,
                "Volunteer {VolunteerId} added photos to pet {PetId}",
                command.VolunteerId,
                command.PetId);

            return command.VolunteerId;
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
}