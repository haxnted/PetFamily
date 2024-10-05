using PetFamily.Application.Features.Species.Commands.AddSpecies;

namespace PetFamily.API.Controllers.SpeciesController.Requests;

public record AddSpeciesRequest(string TypeAnimal)
{
    public AddSpeciesCommand ToCommand() =>
        new AddSpeciesCommand(TypeAnimal);
}