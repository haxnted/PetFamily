using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.VolunteerManagement;
using PetFamily.Domain.VolunteerManagement.ValueObjects;

namespace PetFamily.Application.Features.VolunteerManagement.Commands.CreateVolunteer;

public class CreateVolunteerHandler(
    IUnitOfWork unitOfWork,
    IVolunteersRepository volunteersRepository,
    IValidator<CreateVolunteerCommand> validator,
    ILogger<CreateVolunteerHandler> logger) : ICommandHandler<Guid, CreateVolunteerCommand>
{
    public async Task<Result<Guid, ErrorList>> Execute(
        CreateVolunteerCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToList();

        var phoneNumber = PhoneNumber.Create(command.Number);

        var volunteer = await volunteersRepository.GetByPhoneNumber(phoneNumber.Value, cancellationToken);

        if (volunteer.IsSuccess)
            return Errors.Model.AlreadyExist("Volunteer").ToErrorList();

        var volunteerId = VolunteerId.NewId();

        var fullName = FullName.Create(
            command.FullName.Name,
            command.FullName.Surname,
            command.FullName.Patronymic);

        var description = Description.Create(command.Description);

        var ageExperience = AgeExperience.Create(command.AgeExperience);

        var socialLinks = command.SocialLinks
            .Select(x => SocialLink.Create(x.Name, x.Url))
            .Select(x => x.Value);

        var requisites = command.Requisites
            .Select(x => Requisite.Create(x.Name, x.Description))
            .Select(x => x.Value);

        var volunteerResult = new Volunteer(volunteerId,
            fullName.Value,
            description.Value,
            ageExperience.Value,
            phoneNumber.Value,
            new List<SocialLink>(socialLinks),
            new List<Requisite>(requisites));

        logger.Log(LogLevel.Information, "Created new volunteer: {VolunteerId}", volunteerId);

        await volunteersRepository.Add(volunteerResult, cancellationToken);
        await unitOfWork.SaveChanges(cancellationToken);
        
        return volunteerId.Id;
    }
}