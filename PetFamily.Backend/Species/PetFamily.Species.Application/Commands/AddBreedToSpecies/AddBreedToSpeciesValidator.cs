using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel.EntityIds;
using PetFamily.Species.Domain.Entities;

namespace PetFamily.Species.Application.Commands.AddBreedToSpecies;

public class AddBreedToSpeciesValidator : AbstractValidator<AddBreedToSpeciesCommand>
{
    public AddBreedToSpeciesValidator()
    {
        RuleFor(b => new {b.SpeciesId , b.Breed})
            .MustBeValueObject(b => Breed.Create(BreedId .Create(b.SpeciesId), b.Breed));
    }
}