using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Species.Commands.DeleteSpecies;

public class DeleteSpeciesValidator : AbstractValidator<DeleteSpeciesCommand>
{
    public DeleteSpeciesValidator()
    {
        RuleFor(s => s.SpeciesId)
            .NotEmpty().WithError(Errors.General.ValueIsRequired("SpeciesId"));
    }
}