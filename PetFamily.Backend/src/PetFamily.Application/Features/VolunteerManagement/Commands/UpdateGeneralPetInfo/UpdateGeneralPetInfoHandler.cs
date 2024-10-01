using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Extensions;
using PetFamily.Application.Features.Species;
using PetFamily.Application.Features.VolunteerManagement.Commands.AddPet;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.VolunteerManagement.Entities;
using PetFamily.Domain.VolunteerManagement.ValueObjects;

namespace PetFamily.Application.Features.VolunteerManagement.Commands.UpdateGeneralPetInfo;

public class UpdateGeneralPetInfoHandler(
    IVolunteersRepository volunteersRepository,
    ISpeciesRepository speciesRepository,
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
        
        var speciesId = SpeciesId.Create(command.SpeciesId);
        var species = await speciesRepository.GetSpeciesById(speciesId, cancellationToken);
        if (species.IsFailure)
            return species.Error.ToErrorList();
        
        var breedId = species.Value.Breeds.FirstOrDefault(s => s.Id == command.BreedId);
        if (breedId is null)
            return Errors.General.NotFound(command.BreedId).ToErrorList();
        
        
        var pet = TryCreatePet(command).Value;
        volunteer.Value.AddPet(pet);

        var result = await volunteersRepository.Save(volunteer.Value, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToErrorList();

        logger.Log(LogLevel.Information, "Volunteer {VolunteerId} added pet {PetId}", volunteerId, pet.Id);
        return pet.Id.Id;
    }
    private Result<Pet> TryCreatePet(UpdateGeneralPetInfoCommand command)
    {
        var petId = PetId.Create(command.VolunteerId);
        
        var nickName = NickName.Create(command.NickName).Value;
        var generalDescription = Description.Create(command.GeneralDescription).Value;
        var healthDescription = Description.Create(command.HealthDescription).Value;
        var address = Address.Create(
            command.Address.Street,
            command.Address.City,
            command.Address.State,
            command.Address.ZipCode).Value;

        var speciesId = SpeciesId.Create(command.SpeciesId);
        var breedId = BreedId.Create(command.BreedId);
        
        var physicalAttributes = PetPhysicalAttributes.Create(command.Weight, command.Height).Value;
        var phoneNumber = PhoneNumber.Create(command.PhoneNumber).Value;
        var petTimeCreated = DateTime.Now.ToUniversalTime();
        var convertRequisites = command.Requisites
            .Select(r => Requisite.Create(r.Name, r.Description))
            .Select(r => r.Value);

        return new Pet(
            petId,
            nickName,
            generalDescription,
            healthDescription,
            address,
            physicalAttributes,
            speciesId,
            breedId,
            phoneNumber,
            command.BirthDate,
            command.IsCastrated,
            command.IsVaccinated,
            command.HelpStatus,
            petTimeCreated,
            new ValueObjectList<PetPhoto>([]),
            new ValueObjectList<Requisite>(convertRequisites)
        );
    }
}