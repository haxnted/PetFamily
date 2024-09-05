using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Controllers.Volunteers.Requests;
using PetFamily.API.Extensions;
using PetFamily.API.Processors;
using PetFamily.Application.Dto;
using PetFamily.Application.Features.Volunteers.AddFilesPet;
using PetFamily.Application.Features.Volunteers.AddPet;
using PetFamily.Application.Features.Volunteers.CreateVolunteer;
using PetFamily.Application.Features.Volunteers.DeleteVolunteer;
using PetFamily.Application.Features.Volunteers.UpdateRequisites;
using PetFamily.Application.Features.Volunteers.UpdateSocialLinks;
using PetFamily.Application.Features.Volunteers.UpdateVolunteer;

namespace PetFamily.API.Controllers.Volunteers;

public class VolunteersController : ApplicationController
{
    [HttpPost("volunteer")]
    public async Task<ActionResult<Guid>> Create(
        [FromBody] CreateVolunteerRequest request,
        [FromServices] CreateVolunteerHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Execute(request.ToCommand(), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Created(result.Value.ToString(), null);
    }

    [HttpPatch("{id:guid}/main-info")]
    public async Task<ActionResult> UpdateMainInfo(
        [FromRoute] Guid id,
        [FromBody] UpdateVolunteerRequest request,
        [FromServices] UpdateVolunteerHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Execute(request.ToCommand(id), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(new { Message = id });
    }

    [HttpPatch("{id:guid}/social-links")]
    public async Task<ActionResult> UpdateSocialLinks(
        [FromRoute] Guid id,
        [FromBody] IEnumerable<SocialLinkDto> request,
        [FromServices] UpdateSocialLinksHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Execute(new UpdateSocialLinksCommand(id, request), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(id);
    }

    [HttpPatch("{id:guid}/requisites")]
    public async Task<ActionResult> UpdateRequisites(
        [FromRoute] Guid id,
        [FromBody] IEnumerable<RequisiteDto> request,
        [FromServices] UpdateRequisitesHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Execute(new UpdateRequisitesCommand(id, request), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(id);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(
        [FromRoute] Guid id,
        CancellationToken cancellationToken,
        [FromServices] DeleteVolunteerHandler handler)
    {
        var result = await handler.Execute(new DeleteVolunteerCommand(id), cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(id);
    }

    [HttpPost("{id:guid}/pet/photos")]
    public async Task<ActionResult> AddFilesToPet(
        [FromForm] AddPetFilesRequest request,
        [FromRoute] Guid id,
        [FromServices] AddPhotosToPetHandler handler,
        CancellationToken cancellationToken)
    {
        var processor = new FileProcessor();
        var fileContents = processor.Process(request.Files);

        var result = await handler.Execute(request.ToCommand(id, fileContents), cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPost("{id:guid}/pet/general")]
    public async Task<ActionResult> CreatePet(
        [FromBody] AddPetRequest request,
        [FromRoute] Guid id,
        [FromServices] AddPetHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Execute(request.ToCommand(id), cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}