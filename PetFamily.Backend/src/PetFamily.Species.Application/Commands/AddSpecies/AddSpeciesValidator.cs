using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.VolunteerManagement.Domain.ValueObjects;

namespace PetFamily.Species.Application.Commands.AddSpecies;

public class AddSpeciesValidator : AbstractValidator<AddSpeciesCommand>
{
    public AddSpeciesValidator()
    {
        RuleFor(s => s.TypeAnimal)
            .MustBeValueObject(TypeAnimal.Create);
    }
}