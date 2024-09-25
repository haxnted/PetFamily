using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Features.Species.Commands.DeleteSpecies;

public record DeleteSpeciesCommand(Guid SpeciesId) : ICommand;