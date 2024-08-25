using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Volunteers.UpdateVolunteer;

public class UpdateVolunteerValidator : AbstractValidator<UpdateVolunteerRequest>
{
    public UpdateVolunteerValidator()
    {
        RuleFor(v => v.IdVolunteer).NotEmpty();
        
        RuleFor(c => new { c.FullName.Name, c.FullName.Surname, c.FullName.Patronymic })
            .MustBeValueObject(x => FullName.Create(x.Name, x.Surname, x.Patronymic));

        RuleFor(c => c.Description)
            .MustBeValueObject(Description.Create);

        RuleFor(c => c.AgeExperience)
            .MustBeValueObject(AgeExperience.Create);

        RuleFor(c => c.PhoneNumber)
            .MustBeValueObject(PhoneNumber.Create);
    }
}