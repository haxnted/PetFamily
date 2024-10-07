using PetFamily.Species.Application.Commands.DeleteBreed;

namespace PetFamily.Species.Presentation.Requests;

public record DeleteBreedRequest(Guid BreedId)
{
    public DeleteBreedCommand ToCommand(Guid speciesId) =>
        new(speciesId, BreedId);
}