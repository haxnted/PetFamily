using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.VolunteerManagement;

namespace PetFamily.Application.Volunteers.AddFilesPet;

public class AddPhotosToPetHandler(
    IUnitOfWork unitOfWork,
    IVolunteersRepository volunteersRepository,
    IFileProvider provider,
    ILogger<AddPhotosToPetHandler> logger)
{
    public async Task<Result<Guid, Error>> Execute(AddPhotosToPetCommand command, CancellationToken token = default)
    {
        var volunteerId = VolunteerId.Create(command.VolunteerId);
        var volunteer = await volunteersRepository.GetById(volunteerId, token);
        if (volunteer.IsFailure)
            return volunteer.Error;

        var petId = PetId.Create(command.PetId);
        var pet = volunteer.Value.GetPetById(petId);
        if (pet == null)
            return Errors.General.NotFound(petId);

        var transaction = await unitOfWork.BeginTransaction(token);

        try
        {
            var petPhotosConvert = from fileContent in command.Files
                let extensionFile = Path.GetExtension(fileContent.ObjectName)
                let uniquePath = Guid.NewGuid() + extensionFile
                select new FileContent(fileContent.Stream, uniquePath);

            var photosConvert = petPhotosConvert.ToList();
            var petPhotoList = photosConvert
                .Select(file => PetPhoto.Create(file.ObjectName, false))
                .Select(file => file.Value);

            pet.UpdateFiles(new PetPhotoList(petPhotoList));
            await unitOfWork.SaveChanges(token);

            var resultUpload = await provider.UploadFiles(photosConvert, token);
            if (resultUpload.IsFailure)
                return resultUpload.Error;

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

            logger.Log(LogLevel.Information, "Transaction failed. Executed command: {pet}", command);
            return Result.Failure<Guid, Error>(
                Error.Failure("Failed.add.photos", "Failed add photos to pet"));
        }
    }
}