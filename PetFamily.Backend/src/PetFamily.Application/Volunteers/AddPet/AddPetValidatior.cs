using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.VolunteerManagement;

namespace PetFamily.Application.Volunteers.AddPet;

public class AddPetValidator : AbstractValidator<AddPetCommand>
{
    public AddPetValidator()
    {
        RuleFor(p => p.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired("VolunteerId"));

        RuleFor(p => p.NickName)
            .MustBeValueObject(NickName.Create);

        RuleFor(p => p.GeneralDescription)
            .MustBeValueObject(Description.Create);

        RuleFor(p => p.HealthDescription)
            .MustBeValueObject(Description.Create);

        RuleFor(p => new { p.Address.Street, p.Address.City, p.Address.State, p.Address.ZipCode })
            .MustBeValueObject(p => Address.Create(p.Street, p.City, p.State, p.ZipCode));

        RuleFor(p => new { p.Weight, p.Height })
            .MustBeValueObject(p => PetPhysicalAttributes.Create(p.Weight, p.Height));
        
        RuleFor(p => p.PhoneNumber)
            .MustBeValueObject(PhoneNumber.Create);

        RuleFor(p => p.BirthDate);

        RuleForEach(p => p.Requisites)
            .MustBeValueObject(p => Requisite.Create(p.Name, p.Description));
    }
}