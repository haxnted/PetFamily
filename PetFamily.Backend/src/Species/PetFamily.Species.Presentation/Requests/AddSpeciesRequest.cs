using PetFamily.Species.Application.Commands.AddSpecies;

namespace PetFamily.Species.Presentation.Requests;

public record AddSpeciesRequest(string TypeAnimal)
{
    public AddSpeciesCommand ToCommand() =>
        new AddSpeciesCommand(TypeAnimal);
}