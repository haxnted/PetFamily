using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.DeleteVolunteer;

public class DeleteVolunteerValidator : AbstractValidator<DeleteVolunteerRequest>
{
    public DeleteVolunteerValidator()
    {
        RuleFor(d => d.Id)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired("Id"));
    }
}