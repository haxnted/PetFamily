﻿using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.VolunteerManagement.Domain.ValueObjects;

namespace PetFamily.VolunteerManagement.Application.Commands.UpdateGeneralPetInfo;

public class UpdateGeneralPetInfoValidator : AbstractValidator<UpdateGeneralPetInfoCommand>
{
    public UpdateGeneralPetInfoValidator()
    {
        RuleFor(p => p.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired("VolunteerId"));

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

        RuleForEach(p => p.Requisites)
            .MustBeValueObject(p => Requisite.Create(p.Name, p.Description));
    }
}