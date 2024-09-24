using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.Species.Entities;

namespace PetFamily.Application.Features.Species.Commands.AddBreedToSpecies;

public class AddBreedToSpeciesValidator : AbstractValidator<AddBreedToSpeciesCommand>
{
    public AddBreedToSpeciesValidator()
    {
        RuleFor(b => new {b.SpeciesId , b.Breed})
            .MustBeValueObject(b => Breed.Create(BreedId .Create(b.SpeciesId), b.Breed));
    }
}