using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.VolunteerManagement.ValueObjects;

namespace PetFamily.Application.Features.VolunteerManagement.Commands.CreateVolunteer;

public class CreateVolunteerValidator : AbstractValidator<CreateVolunteerCommand>
{
    public CreateVolunteerValidator()
    {
        RuleFor(c => new { c.FullName.Name, c.FullName.Surname, c.FullName.Patronymic })
            .MustBeValueObject(x => FullName.Create(x.Name, x.Surname, x.Patronymic));

        RuleFor(c => c.Description)
            .MustBeValueObject(Description.Create);

        RuleFor(c => c.AgeExperience)
            .MustBeValueObject(AgeExperience.Create);

        RuleFor(c => c.Number)
            .MustBeValueObject(PhoneNumber.Create);

        RuleForEach(c => c.Requisites)
            .MustBeValueObject(s => Requisite.Create(s.Name, s.Description));

        RuleForEach(c => c.SocialLinks)
            .MustBeValueObject(s => SocialLink.Create(s.Name, s.Url));
    }
}