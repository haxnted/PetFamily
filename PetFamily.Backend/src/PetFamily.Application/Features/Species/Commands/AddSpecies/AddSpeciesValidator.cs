using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.VolunteerManagement.ValueObjects;

namespace PetFamily.Application.Features.Species.Commands.AddSpecies;

public class AddSpeciesValidator : AbstractValidator<AddSpeciesCommand>
{
    public AddSpeciesValidator()
    {
        RuleFor(s => s.TypeAnimal)
            .MustBeValueObject(TypeAnimal.Create);
    }
}