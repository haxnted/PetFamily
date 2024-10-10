using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.Species.Application.Commands.DeleteBreed;

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