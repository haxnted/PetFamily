using PetFamily.Core.Abstractions;

namespace PetFamily.Species.Application.Commands.DeleteSpecies;

public record DeleteSpeciesCommand(Guid SpeciesId) : ICommand;