using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.Application.Volunteers.CreateVolunteer;
using PetFamily.Application.Volunteers.UpdateRequisites;
using PetFamily.Application.Volunteers.UpdateSocialLinks;
using PetFamily.Application.Volunteers.UpdateVolunteer;

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

    [HttpPatch("fullupdate")]
    public async Task<ActionResult> Update([FromServices] UpdateVolunteerHandler service,
        [FromBody] UpdateVolunteerRequest request,
        CancellationToken cancellationToken)
    {
        var result = await service.Execute(request, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(new { Message = "Volunteer updated successfully" });
    }

    [HttpPatch("sociallinks")]
    public async Task<ActionResult> UpdateSocialLinks([FromServices] UpdateSocialLinksHandler service,
        [FromBody] UpdateSocialLinksRequest request,
        CancellationToken cancellationToken)
    {
        var result = await service.Execute(request, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(new { Message = "SocialLinks updated successfully" });
    }

    [HttpPatch("requisites")]
    public async Task<ActionResult> UpdateRequisites([FromServices] UpdateRequisitesHandler service,
        [FromBody] UpdateRequisitesRequest request,
        CancellationToken cancellationToken)
    {
        var result = await service.Execute(request, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(new { Message = "Requisites updated successfully" });
    }
}