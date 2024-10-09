using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.EntityIds;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.VolunteerManagement.Domain;
using PetFamily.VolunteerManagement.Domain.ValueObjects;

namespace PetFamily.VolunteerManagement.Application.Commands.CreateVolunteer;

public class CreateVolunteerHandler(
    IVolunteerUnitOfWork unitOfWork,
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
            [..socialLinks],
            [..requisites]);

        logger.Log(LogLevel.Information, "Created new volunteer: {VolunteerId}", volunteerId);

        await volunteersRepository.Add(volunteerResult, cancellationToken);
        await unitOfWork.SaveChanges(cancellationToken);
        
        return volunteerId.Id;
    }
}