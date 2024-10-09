using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.Species.Domain.ValueObjects;

namespace PetFamily.Species.Application.Commands.AddSpecies;

public class AddSpeciesValidator : AbstractValidator<AddSpeciesCommand>
{
    public AddSpeciesValidator()
    {
        RuleFor(s => s.TypeAnimal)
            .MustBeValueObject(TypeAnimal.Create);
    }
}