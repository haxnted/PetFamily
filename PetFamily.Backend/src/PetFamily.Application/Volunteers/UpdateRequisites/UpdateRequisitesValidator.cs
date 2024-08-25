using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Аggregate.Volunteer;

namespace PetFamily.Application.Volunteers.UpdateRequisites;

public class UpdateRequisitesValidator : AbstractValidator<UpdateRequisitesRequest>
{
    public UpdateRequisitesValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
        
        RuleForEach(c => c.Requisites)
            .MustBeValueObject(s => Requisite.Create(s.Name, s.Description));
    }
}