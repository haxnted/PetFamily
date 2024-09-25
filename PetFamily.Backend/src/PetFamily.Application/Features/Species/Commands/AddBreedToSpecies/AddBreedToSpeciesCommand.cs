using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Features.Species.Commands.AddBreedToSpecies;

public record AddBreedToSpeciesCommand(Guid SpeciesId, string Breed) : ICommand;