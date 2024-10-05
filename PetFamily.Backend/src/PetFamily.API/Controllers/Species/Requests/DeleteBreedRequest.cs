using PetFamily.Application.Features.Species.Commands.DeleteBreed;

namespace PetFamily.API.Controllers.Species.Requests;

public record DeleteBreedRequest(Guid BreedId)
{
    public DeleteBreedCommand ToCommand(Guid speciesId) =>
        new(speciesId, BreedId);
}