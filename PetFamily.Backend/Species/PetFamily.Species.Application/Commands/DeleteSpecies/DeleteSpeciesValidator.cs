using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.Species.Application.Commands.DeleteSpecies;

public class DeleteSpeciesValidator : AbstractValidator<DeleteSpeciesCommand>
{
    public DeleteSpeciesValidator()
    {
        RuleFor(s => s.SpeciesId)
            .NotEmpty().WithError(Errors.General.ValueIsRequired("SpeciesId"));
    }
}