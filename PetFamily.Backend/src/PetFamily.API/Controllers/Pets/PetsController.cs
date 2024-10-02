using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Controllers.Pets.Requests;
using PetFamily.API.Extensions;
using PetFamily.Application.Features.VolunteerManagement.Queries.GetAllPetsWithPagination;

namespace PetFamily.API.Controllers.Pets;

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
}