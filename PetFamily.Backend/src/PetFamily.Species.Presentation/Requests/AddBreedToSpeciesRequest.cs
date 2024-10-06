using PetFamily.Species.Application.Commands.AddBreedToSpecies;

namespace PetFamily.Species.Presentation.Requests;

public record AddBreedToSpeciesRequest(string Breed)
{
    public AddBreedToSpeciesCommand ToCommand(Guid speciesId) =>
        new(speciesId, Breed);
}