using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Features.Species.Commands.AddSpecies;

public record AddSpeciesCommand(string TypeAnimal) : ICommand;