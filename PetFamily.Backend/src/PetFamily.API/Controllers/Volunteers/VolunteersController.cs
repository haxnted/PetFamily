using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.API.Processors;
using PetFamily.Application.Dto;
using PetFamily.Application.Features.VolunteerManagement.Commands.AddFilesPet;
using PetFamily.Application.Features.VolunteerManagement.Commands.AddPet;
using PetFamily.Application.Features.VolunteerManagement.Commands.CreateVolunteer;
using PetFamily.Application.Features.VolunteerManagement.Commands.DeleteVolunteer;
using PetFamily.Application.Features.VolunteerManagement.Commands.RemoveFilesFromPet;
using PetFamily.Application.Features.VolunteerManagement.Commands.RemoveHardPetById;
using PetFamily.Application.Features.VolunteerManagement.Commands.RemoveSoftPetById;
using PetFamily.Application.Features.VolunteerManagement.Commands.UpdateGeneralPetInfo;
using PetFamily.Application.Features.VolunteerManagement.Commands.UpdatePositionPet;
using PetFamily.Application.Features.VolunteerManagement.Commands.UpdateRequisites;
using PetFamily.Application.Features.VolunteerManagement.Commands.UpdateSocialLinks;
using PetFamily.Application.Features.VolunteerManagement.Commands.UpdateVolunteer;
using PetFamily.Application.Features.VolunteerManagement.Queries.GetVolunteerById;
using PetFamily.Application.Features.VolunteerManagement.Queries.GetVolunteersWithPagination;

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
    
    [HttpPost("{volunteerId:guid}/pet/{petId:guid}/photos")]
    public async Task<ActionResult> AddFilesToPet(
        [FromForm] AddPetFilesRequest request,
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromServices] AddPhotosToPetHandler handler,
        CancellationToken cancellationToken)
    {
        var processor = new FileProcessor();
        var fileContents = processor.Process(request.Files);

        var result = await handler.Execute(request.ToCommand(volunteerId, petId, fileContents), cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [HttpPost("{volunteerId:guid}/pet/general")]
    public async Task<ActionResult> CreatePet(
        [FromBody] AddPetRequest request,
        [FromRoute] Guid volunteerId,
        [FromServices] AddPetHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Execute(request.ToCommand(volunteerId), cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    [HttpPatch("{volunteerId:guid}/main-info")]
    public async Task<ActionResult> UpdateMainInfo(
        [FromRoute] Guid volunteerId,
        [FromBody] UpdateVolunteerRequest request,
        [FromServices] UpdateVolunteerHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Execute(request.ToCommand(volunteerId), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(new { Message = volunteerId });
    }

    [HttpPatch("{volunteerId:guid}/social-links")]
    public async Task<ActionResult> UpdateSocialLinks(
        [FromRoute] Guid volunteerId,
        [FromBody] IEnumerable<SocialLinkDto> request,
        [FromServices] UpdateSocialLinksHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Execute(new UpdateSocialLinksCommand(volunteerId, request), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(volunteerId);
    }

    [HttpPatch("{volunteerId:guid}/requisites")]
    public async Task<ActionResult> UpdateRequisites(
        [FromRoute] Guid volunteerId,
        [FromBody] IEnumerable<RequisiteDto> request,
        [FromServices] UpdateRequisitesHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Execute(new UpdateRequisitesCommand(volunteerId, request), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(volunteerId);
    }
    

    [HttpPatch("{volunteerId:guid}/pet/{petId:guid}/general")]
    public async Task<ActionResult> UpdateGeneralInfoPet(
        [FromBody] UpdateGeneralPetInfoRequest request,
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromServices] UpdateGeneralPetInfoHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Execute(request.ToCommand(volunteerId, petId), cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    

    [HttpPatch("{volunteerId:guid}/pet/{petId:guid}/position/{position:int}")]
    public async Task<ActionResult> UpdatePetPosition(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromRoute] int position,
        [FromServices] UpdatePetPositionHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Execute(new UpdatePetPositionCommand(volunteerId, petId, position), cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();


        return Ok(result.Value);
    }

    [HttpDelete("{volunteerId:guid}")]
    public async Task<ActionResult> Delete(
        [FromRoute] Guid volunteerId,
        CancellationToken cancellationToken,
        [FromServices] DeleteVolunteerHandler handler)
    {
        var result = await handler.Execute(new DeleteVolunteerCommand(volunteerId), cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(volunteerId);
    }

    [HttpDelete("{volunteerId:guid}/pet/{petId:guid}/soft")]
    public async Task<ActionResult> SoftDeletePet(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        CancellationToken cancellationToken,
        [FromServices] RemoveSoftPetByIdHandler handler)
    {
        var result = await handler.Execute(new RemoveSoftPetByIdCommand(volunteerId, petId), cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(volunteerId);
    }
    
    [HttpDelete("{volunteerId:guid}/pet/{petId:guid}/hard")]
    public async Task<ActionResult> SoftDeletePet(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        CancellationToken cancellationToken,
        [FromServices] RemoveHardPetByIdHandler handler)
    {
        var result = await handler.Execute(new RemoveHardPetByIdCommand(volunteerId, petId), cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(volunteerId);
    }
    
    [HttpDelete("{volunteerId:guid}/pet/{petId:guid}/photos")]
    public async Task<ActionResult> RemoveFilesFromPet(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromForm] RemoveFilesFromPetRequest request,
        [FromServices] RemoveFilesFromPetHandler handler,
        CancellationToken cancellationToken
    )
    {
        var result = await handler.Execute(request.ToCommand(volunteerId, petId), cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [HttpGet]
    public async Task<ActionResult> GetVolunteersWithPagination(
        [FromQuery] GetVolunteersWithPaginationRequest request,
        [FromServices] GetVolunteersWithPaginationHandler handler,
        CancellationToken token)
    {
        var result = await handler.Execute(request.ToQuery(), token);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpGet("{volunteerId:guid}")]
    public async Task<ActionResult> GetVolunteerById(
        [FromRoute] Guid volunteerId,
        [FromServices] GetVolunteerByIdHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new GetVolunteerByIdCommand(volunteerId);
        var result = await handler.Execute(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}