using System.Windows.Input;
using PetFamily.Core.Abstractions;
using ICommand = PetFamily.Core.Abstractions.ICommand;

namespace PetFamily.Species.Application.Commands.AddBreedToSpecies;

public record AddBreedToSpeciesCommand(Guid SpeciesId, string Breed) : ICommand;