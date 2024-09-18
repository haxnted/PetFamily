using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteerManagement.ValueObjects;

namespace PetFamily.Application.Features.VolunteerManagement.Commands.UpdateRequisites;

public class UpdateRequisitesValidator : AbstractValidator<UpdateRequisitesCommand>
{
    public UpdateRequisitesValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired("Id"));
        
        RuleForEach(c => c.Requisites)
            .MustBeValueObject(s => Requisite.Create(s.Name, s.Description));
    }
}