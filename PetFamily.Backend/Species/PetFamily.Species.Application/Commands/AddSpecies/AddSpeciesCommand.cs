using PetFamily.Core.Abstractions;

namespace PetFamily.Species.Application.Commands.AddSpecies;

public record AddSpeciesCommand(string TypeAnimal) : ICommand;