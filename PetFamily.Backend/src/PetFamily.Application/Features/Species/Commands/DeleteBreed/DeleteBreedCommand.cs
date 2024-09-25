using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Features.Species.Commands.DeleteBreed;

public record DeleteBreedCommand(Guid SpeciesId, Guid BreedId) : ICommand;