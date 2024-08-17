using Microsoft.AspNetCore.Mvc;
using PetFamily.Application.Volunteers.CreateVolunteer;

namespace PetFamily.API.Controllers;

[ApiController]
[Route("[controller]")]
public class VolunteersController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromServices] CreateVolunteerHandler service, 
        [FromBody] CreateVolunteerRequest request,
        CancellationToken cancellationToken = default)
    {
       var result = await service.Execute(request, cancellationToken);
       if (result.IsFailure)
       {
           return BadRequest();
       }

       return Ok(result.Value);
    }
}

