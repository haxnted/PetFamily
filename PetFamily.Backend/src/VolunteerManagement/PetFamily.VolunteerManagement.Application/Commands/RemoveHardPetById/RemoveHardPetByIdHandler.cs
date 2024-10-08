using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Core.Providers.FileProvider;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.EntityIds;

namespace PetFamily.VolunteerManagement.Application.Commands.RemoveHardPetById;

public class RemoveHardPetByIdHandler(
    IVolunteerUnitOfWork unitOfWork,
    IFileProvider fileProvider,
    IVolunteersRepository volunteersRepository,
    IValidator<RemoveHardPetByIdCommand> validator,
    ILogger<RemoveHardPetByIdCommand> logger
) : ICommandHandler<Guid, RemoveHardPetByIdCommand>
{
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

        foreach (var path in petMediaPaths)
        {
            var file = await fileProvider.GetFileByName(path, 
                Constants.BUCKET_NAME_FOR_PET_IMAGES,
                cancellationToken);
            
            if (file.IsSuccess)
            {
                await fileProvider.Delete(path, Constants.BUCKET_NAME_FOR_PET_IMAGES, cancellationToken);
            }
        }


        logger.Log(LogLevel.Information, "Pet {pet} was removed", pet);

        return command.PetId;
    }
}