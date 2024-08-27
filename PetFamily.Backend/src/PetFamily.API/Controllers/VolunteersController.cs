using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.Application.Dto;
using PetFamily.Application.Volunteers.CreateVolunteer;
using PetFamily.Application.Volunteers.UpdateRequisites;
using PetFamily.Application.Volunteers.UpdateSocialLinks;
using PetFamily.Application.Volunteers.UpdateVolunteer;

namespace PetFamily.API.Controllers;

public class VolunteersController : ApplicationController
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromServices] CreateVolunteerHandler handler,
        [FromBody] CreateVolunteerRequest request,
        CancellationToken cancellationToken)
    {
        var result = await handler.Execute(request, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Created(result.Value.ToString(), null);
    }

    [HttpPut("{id:guid}/main-info")]
    public async Task<ActionResult> Update([FromServices] UpdateVolunteerHandler handler,
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

        return Ok(new { Message = "Volunteer updated successfully" });
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

        return Ok(new { Message = "SocialLinks updated successfully" });
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

        return Ok(new { Message = "Requisites updated successfully" });
    }
}