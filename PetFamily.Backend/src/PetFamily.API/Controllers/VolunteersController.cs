using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.Application.Volunteers.CreateVolunteer;

namespace PetFamily.API.Controllers;

[ApiController]
[Route("[controller]")]
public class VolunteersController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromServices] CreateVolunteerHandler service, 
        [FromBody] CreateVolunteerRequest request,
        CancellationToken cancellationToken)
    {
       var result = await service.Execute(request, cancellationToken);
       
       if (result.IsFailure)
           return result.Error.ToResponse();

       return Created(result.Value.ToString(), null);
    }
}

