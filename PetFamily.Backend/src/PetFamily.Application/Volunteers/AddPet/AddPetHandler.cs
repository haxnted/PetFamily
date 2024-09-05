using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.VolunteerManagement;

namespace PetFamily.Application.Volunteers.AddPet;

public class AddPetHandler(
    IFileProvider provider,
    IVolunteersRepository repository,
    IUnitOfWork unitOfWork,
    ILogger<AddPetHandler> logger)
{
    public async Task<Result<Guid, Error>> Execute(AddPetCommand command,
        CancellationToken token = default)
    {
        var volunteerId = VolunteerId.Create(command.VolunteerId);
        var volunteer = await repository.GetById(volunteerId, token);
        if (volunteer.IsFailure)
            return volunteer.Error;

        var pet = TryCreatePet(command).Value;
        volunteer.Value.AddPet(pet);

        var result = await repository.Save(volunteer.Value, token);
        if (result.IsFailure)
            return result.Error;

        logger.Log(LogLevel.Information, "Volunteer {VolunteerId} added pet {PetId}", volunteerId, pet.Id);
        return command.VolunteerId;
    }

    private Result<Pet> TryCreatePet(AddPetCommand command)
    {
        var petId = PetId.NewId();
        var breedId = BreedId.NewId();
        var nickName = NickName.Create(command.NickName).Value;
        var generalDescription = Description.Create(command.GeneralDescription).Value;
        var healthDescription = Description.Create(command.HealthDescription).Value;
        var address = Address.Create(
            command.Address.Street,
            command.Address.City,
            command.Address.State,
            command.Address.ZipCode).Value;
        var physicalAttributes = PetPhysicalAttributes.Create(command.Weight, command.Height).Value;
        var phoneNumber = PhoneNumber.Create(command.PhoneNumber).Value;
        var petTimeCreated = DateTime.Now.ToUniversalTime();
        var convertRequisites = command.Requisites
            .Select(r => Requisite.Create(r.Name, r.Description))
            .Select(r => r.Value);
        var requisiteList = new RequisiteList(convertRequisites.ToList());

        return new Pet(
            petId,
            nickName,
            generalDescription,
            healthDescription,
            address,
            physicalAttributes,
            Guid.Empty,
            breedId,
            phoneNumber,
            command.BirthDate,
            command.IsCastrated,
            command.IsVaccinated,
            command.HelpStatus,
            petTimeCreated,
            new PetPhotoList([]),
            requisiteList
        );
    }
}