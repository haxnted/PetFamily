using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Contracts;
using PetFamily.API.Extensions;
using PetFamily.API.Processors;
using PetFamily.Application.Dto;
using PetFamily.Application.Volunteers.AddFilesPet;
using PetFamily.Application.Volunteers.AddPet;
using PetFamily.Application.Volunteers.CreateVolunteer;
using PetFamily.Application.Volunteers.DeleteVolunteer;
using PetFamily.Application.Volunteers.UpdateRequisites;
using PetFamily.Application.Volunteers.UpdateSocialLinks;
using PetFamily.Application.Volunteers.UpdateVolunteer;

namespace PetFamily.API.Controllers;

public class VolunteersController : ApplicationController
{
    [HttpPost("volunteer")]
    public async Task<ActionResult<Guid>> Create([FromServices] CreateVolunteerHandler handler,
        [FromBody] CreateVolunteerRequest request,
        CancellationToken cancellationToken)
    {
        var result = await handler.Execute(request, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Created(result.Value.ToString(), null);
    }

    [HttpPatch("{id:guid}/main-info")]
    public async Task<ActionResult> UpdateMainInfo([FromServices] UpdateVolunteerHandler handler,
        [FromServices] IValidator<UpdateVolunteerRequest> validator,
        [FromRoute] Guid id,
        [FromBody] UpdateVolunteerDto request,
        CancellationToken cancellationToken)
    {
        var updateVolunteerRequest = new UpdateVolunteerRequest(id,
            request.FullName,
            request.Description,
            request.AgeExperience,
            request.PhoneNumber);

        var validateResult = await validator.ValidateAsync(updateVolunteerRequest, cancellationToken);
        if (validateResult.IsValid == false)
            return validateResult.ToValidationErrorResponse();

        var result = await handler.Execute(updateVolunteerRequest, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(new { Message = id });
    }

    [HttpPatch("{id:guid}/social-links")]
    public async Task<ActionResult> UpdateSocialLinks([FromServices] UpdateSocialLinksHandler handler,
        [FromServices] IValidator<UpdateSocialLinksRequest> validator,
        [FromRoute] Guid id,
        [FromBody] UpdateSocialLinksDto request,
        CancellationToken cancellationToken)
    {
        var updateSocialLinksRequest = new UpdateSocialLinksRequest(id, request.SocialLinks);

        var validateResult = await validator.ValidateAsync(updateSocialLinksRequest, cancellationToken);
        if (validateResult.IsValid == false)
            return validateResult.ToValidationErrorResponse();

        var result = await handler.Execute(updateSocialLinksRequest, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(new { Message = id });
    }

    [HttpPatch("{id:guid}/requisites")]
    public async Task<ActionResult> UpdateRequisites([FromServices] UpdateRequisitesHandler handler,
        [FromServices] IValidator<UpdateRequisitesRequest> validator,
        [FromRoute] Guid id,
        [FromBody] UpdateRequisitesDto request,
        CancellationToken cancellationToken)
    {
        var updateRequisitesRequest = new UpdateRequisitesRequest(id, request.Requisites);

        var validateResult = await validator.ValidateAsync(updateRequisitesRequest, cancellationToken);
        if (validateResult.IsValid == false)
            return validateResult.ToValidationErrorResponse();

        var result = await handler.Execute(updateRequisitesRequest, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(new { Message = id });
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete([FromServices] DeleteVolunteerHandler handler,
        [FromServices] IValidator<DeleteVolunteerRequest> validator,
        CancellationToken cancellationToken,
        [FromRoute] Guid id)
    {
        var deleteVolunteerRequest = new DeleteVolunteerRequest(id);

        var validateResult = await validator.ValidateAsync(deleteVolunteerRequest, cancellationToken);
        if (validateResult.IsValid == false)
            return validateResult.ToValidationErrorResponse();

        var result = await handler.Execute(deleteVolunteerRequest, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(new { Message = id });
    }

    [HttpPost("{id:guid}/pet-files")]
    public async Task<ActionResult> AddFilesToPet([FromForm] AddPetFilesRequest request,
        [FromServices] IValidator<AddPetFilesCommand> validator,
        [FromServices] AddPetFilesHandler handler,
        CancellationToken cancellationToken,
        [FromRoute] Guid id)
    {
        var processor = new FileProcessor();
        var fileContents = processor.Process(request.Files);

        var addPetFilesCommand = new AddPetFilesCommand(id, request.PetId, fileContents, request.IdxMainFile);
        var validateResult = await validator.ValidateAsync(addPetFilesCommand, cancellationToken);
        if (validateResult.IsValid == false)
            return validateResult.ToValidationErrorResponse();

        var result = await handler.Execute(addPetFilesCommand, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }

    [HttpPost("{id:guid}/pet-general")]
    public async Task<ActionResult> CreatePet([FromBody] AddPetRequest request,
        [FromServices] IValidator<AddPetCommand> validator,
        [FromServices] AddPetHandler handler,
        CancellationToken cancellationToken,
        [FromRoute] Guid id)
    {
        

        var addPetCommand = new AddPetCommand(id,
            request.NickName,
            request.GeneralDescription,
            request.HealthDescription,
            request.Address,
            request.Weight,
            request.Height,
            request.PhoneNumber,
            request.BirthDate,
            request.IsCastrated,
            request.IsVaccinated,
            request.HelpStatus,
            request.Requisites);

        var validateResult = await validator.ValidateAsync(addPetCommand, cancellationToken);
        if (validateResult.IsValid == false)
            return validateResult.ToValidationErrorResponse();

        var result = await handler.Execute(addPetCommand, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }
}