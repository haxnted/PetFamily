using PetFamily.Application.Features.Species.Commands.AddBreedToSpecies;

namespace PetFamily.API.Controllers.Species.Requests;

public record AddBreedToSpeciesRequest(string Breed)
{
    public AddBreedToSpeciesCommand ToCommand(Guid speciesId) =>
        new(speciesId, Breed);
}