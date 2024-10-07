using Microsoft.AspNetCore.Mvc;
using PetFamily.Core.Dto;
using PetFamily.Framework;
using PetFamily.VolunteerManagement.Application.Commands.AddFilesPet;
using PetFamily.VolunteerManagement.Application.Commands.AddPet;
using PetFamily.VolunteerManagement.Application.Commands.CreateVolunteer;
using PetFamily.VolunteerManagement.Application.Commands.DeleteVolunteer;
using PetFamily.VolunteerManagement.Application.Commands.RemoveFilesFromPet;
using PetFamily.VolunteerManagement.Application.Commands.RemoveHardPetById;
using PetFamily.VolunteerManagement.Application.Commands.RemoveSoftPetById;
using PetFamily.VolunteerManagement.Application.Commands.UpdateGeneralPetInfo;
using PetFamily.VolunteerManagement.Application.Commands.UpdatePositionPet;
using PetFamily.VolunteerManagement.Application.Commands.UpdateRequisites;
using PetFamily.VolunteerManagement.Application.Commands.UpdateSocialLinks;
using PetFamily.VolunteerManagement.Application.Commands.UpdateVolunteer;
using PetFamily.VolunteerManagement.Application.Queries.GetVolunteerById;
using PetFamily.VolunteerManagement.Application.Queries.GetVolunteersWithPagination;
using PetFamily.VolunteerManagement.Presentation.Processors;
using PetFamily.VolunteerManagement.Presentation.Volunteers.Requests;

namespace PetFamily.VolunteerManagement.Presentation.Volunteers;

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

        return Ok(result.Value);
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