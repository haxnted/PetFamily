using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Volunteers.DeleteVolunteer;

public class DeleteVolunteerValidator : AbstractValidator<DeleteVolunteerCommand>
{
    public DeleteVolunteerValidator()
    {
        RuleFor(d => d.Id)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired("Id"));
    }
}