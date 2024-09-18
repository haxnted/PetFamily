using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.VolunteerManagement.Commands.DeleteVolunteer;

public class DeleteVolunteerValidator : AbstractValidator<DeleteVolunteerCommand>
{
    public DeleteVolunteerValidator()
    {
        RuleFor(d => d.Id)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired("Id"));
    }
}