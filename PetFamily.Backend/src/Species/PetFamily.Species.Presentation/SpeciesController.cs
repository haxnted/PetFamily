using Microsoft.AspNetCore.Mvc;
using PetFamily.Framework;
using PetFamily.Species.Application.Commands.AddBreedToSpecies;
using PetFamily.Species.Application.Commands.AddSpecies;
using PetFamily.Species.Application.Commands.DeleteBreed;
using PetFamily.Species.Application.Commands.DeleteSpecies;
using PetFamily.Species.Application.Queries.GetAllSpeciesWithPagination;
using PetFamily.Species.Application.Queries.GetBreedsBySpecial;
using PetFamily.Species.Presentation.Requests;

namespace PetFamily.Species.Presentation;

public class SpeciesController : ApplicationController
{
    [HttpPost]
    public async Task<ActionResult> Create(
        [FromBody] AddSpeciesRequest request,
        [FromServices] AddSpeciesHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Execute(request.ToCommand(), cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPost("{speciesId:guid}/breed")]
    public async Task<ActionResult> AddBreedToSpecial(
        [FromRoute] Guid speciesId,
        [FromBody] AddBreedToSpeciesRequest request,
        [FromServices] AddBreedToSpeciesHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Execute(request.ToCommand(speciesId), cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }


    [HttpDelete]
    public async Task<ActionResult> RemoveSpecies(
        [FromQuery] Guid id,
        [FromServices] DeleteSpeciesHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Execute(new DeleteSpeciesCommand(id), cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpDelete("{speciesId:guid}/breed")]
    public async Task<ActionResult> RemoveBreedFromSpecies(
        [FromRoute] Guid speciesId,
        [FromBody] DeleteBreedRequest request,
        [FromServices] DeleteBreedHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Execute(request.ToCommand(speciesId), cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpGet]
    public async Task<ActionResult> GetSpeciesWithPagination(
        [FromQuery] GetSpeciesWithPaginationRequest request,
        [FromServices] GetSpeciesWithPaginationHandler handler,
        CancellationToken token)
    {
        var result = await handler.Execute(request.ToQuery(), token);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpGet("breeds")]
    public async Task<ActionResult> GetBreedsBySpecies(
        [FromQuery] GetBreedsBySpecialRequest request,
        [FromServices] GetBreedsBySpecialHandler handler,
        CancellationToken token)
    {
        var result = await handler.Execute(request.ToQuery(), token);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}