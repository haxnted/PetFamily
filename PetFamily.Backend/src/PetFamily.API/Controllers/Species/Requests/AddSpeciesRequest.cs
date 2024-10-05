using PetFamily.Application.Features.Species.Commands.AddSpecies;

namespace PetFamily.API.Controllers.Species.Requests;

public record AddSpeciesRequest(string TypeAnimal)
{
    public AddSpeciesCommand ToCommand() =>
        new AddSpeciesCommand(TypeAnimal);
}