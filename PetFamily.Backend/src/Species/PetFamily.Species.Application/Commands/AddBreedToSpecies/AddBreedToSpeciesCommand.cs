using PetFamily.Core.Abstractions;

namespace PetFamily.Species.Application.Commands.AddBreedToSpecies;

public record AddBreedToSpeciesCommand(Guid SpeciesId, string Breed) : ICommand;