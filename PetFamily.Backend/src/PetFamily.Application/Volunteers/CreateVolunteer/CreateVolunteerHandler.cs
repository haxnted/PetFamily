using System.Collections;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.VolunteerManagement;

namespace PetFamily.Application.Volunteers.CreateVolunteer;

public class CreateVolunteerHandler(IVolunteersRepository repository, ILogger<CreateVolunteerHandler> logger)
{
    public async Task<Result<Guid, Error>> Execute(
        CreateVolunteerRequest request, CancellationToken token = default
    )
    {
        var phoneNumber = PhoneNumber.Create(request.Number);

        var volunteer = await repository.GetByPhoneNumber(phoneNumber.Value, token);

        if (volunteer.IsSuccess)
            return Errors.Model.AlreadyExist("VolunteerManagement");

        var volunteerId = VolunteerId.NewId();

        var fullName = FullName.Create(request.FullName.Name, request.FullName.Surname, request.FullName.Patronymic);

        var description = Description.Create(request.Description);

        var ageExperience = AgeExperience.Create(request.AgeExperience);

        var socialLinks = request.SocialLinks
            .Select(x => SocialLink.Create(x.Name, x.Url))
            .Select(x => x.Value);
        var socialLinksList = new SocialLinksList(socialLinks);

        var requisites = request.Requisites
            .Select(x => Requisite.Create(x.Name, x.Description))
            .Select(x => x.Value);
        var requisitesList = new RequisitesList(requisites);

        var volunteerResult = new Volunteer(volunteerId,
            fullName.Value, description.Value,
            ageExperience.Value, phoneNumber.Value, socialLinksList, requisitesList);

        logger.Log(LogLevel.Information, "Created new volunteer: {VolunteerId}", volunteerId);

        return await repository.Add(volunteerResult, token);
    }
}