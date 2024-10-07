using Microsoft.AspNetCore.Mvc;
using PetFamily.Framework;
using PetFamily.VolunteerManagement.Application.Queries.GetAllPetsWithPagination;
using PetFamily.VolunteerManagement.Application.Queries.GetPetById;
using PetFamily.VolunteerManagement.Presentation.Pets.Requests;

namespace PetFamily.VolunteerManagement.Presentation.Pets;

public class PetsController : ApplicationController
{
    [HttpGet]
    public async Task<ActionResult> GetAllPetsWithPagination(
        [FromQuery] GetAllPetsWithPaginationRequest request,
        [FromServices] GetAllPetsWithPaginationHandler handler,
        CancellationToken token)
    {
        var result = await handler.Execute(request.ToQuery(), token);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [HttpGet("{petId:guid}")]
    public async Task<ActionResult> GetPetById(
        [FromRoute] Guid petId,
        [FromServices] GetPetByIdHandler handler,
        CancellationToken token)
    {
        var result = await handler.Execute(new GetPetByIdQuery(petId), token);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}