using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Species.Commands.DeleteBreed;

public class DeleteBreedValidator : AbstractValidator<DeleteBreedCommand>
{
    public DeleteBreedValidator()
    {
        RuleFor(b => b.SpeciesId)
            .NotEmpty().WithError(Errors.General.ValueIsRequired("SpeciesId"));
        
        RuleFor(b => b.BreedId)
            .NotEmpty().WithError(Errors.General.ValueIsRequired("BreedId"));
    }
}