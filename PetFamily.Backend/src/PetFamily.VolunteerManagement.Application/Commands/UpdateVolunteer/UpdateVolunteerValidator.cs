using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.VolunteerManagement.Application.Commands.UpdateVolunteer;

public class UpdateVolunteerValidator : AbstractValidator<UpdateVolunteerCommand>
{
    public UpdateVolunteerValidator()
    {
        RuleFor(v => v.IdVolunteer)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired("Id"));
        
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