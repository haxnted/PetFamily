using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Аggregate.Volunteer;

namespace PetFamily.Application.Volunteers.CreateVolunteer;

public class CreateVolunteerHandler(IVolunteersRepository repository)
{
    public async Task<Result<Guid, Error>> Execute(
        CreateVolunteerRequest request, CancellationToken token = default
    )
    {
        var phoneNumber = PhoneNumber.Create(request.Number);

        var volunteer = await repository.GetByPhoneNumber(phoneNumber.Value);

        if (volunteer.IsSuccess)
            return Errors.Model.AlreadyExist("Volunteer");

        var volunteerId = VolunteerId.NewId();

        var fullName = FullName.Create(request.Name, request.Surname, request.Patronymic);

        var description = Description.Create(request.Description);

        var ageExperience = AgeExperience.Create(request.AgeExperience);

        var socialLinks = request.SocialLinks
            .Select(x => SocialLink.Create(x.Name, x.Url))
            .ToList();

        var requisites = request.Requisites
            .Select(x => Requisite.Create(x.Name, x.Description))
            .ToList();
        
        var details = new VolunteerDetails(socialLinks.Select(x => x.Value).ToList(),
            requisites.Select(x => x.Value).ToList());

        var volunteerResult = new Volunteer(volunteerId,
            fullName.Value, description.Value,
            ageExperience.Value, phoneNumber.Value, details);

        return await repository.Add(volunteerResult, token);
    }
}