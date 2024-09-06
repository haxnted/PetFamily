using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.VolunteerManagement.ValueObjects;

namespace PetFamily.Application.Features.Volunteers.UpdateSocialLinks;

public class UpdateSocialLinksHandler(
    IVolunteersRepository repository, 
    IValidator<UpdateSocialLinksCommand> validator,
    ILogger<UpdateSocialLinksHandler> logger)
{
    public async Task<Result<Guid, ErrorList>> Execute(UpdateSocialLinksCommand command,
        CancellationToken token = default)
    {
        var validationResult = await validator.ValidateAsync(command, token);
        if (!validationResult.IsValid)
            return validationResult.ToList();
        
        var volunteerId = VolunteerId.Create(command.Id);
        var volunteer = await repository.GetById(volunteerId, token);
        if (volunteer.IsFailure)
            return volunteer.Error.ToErrorList();

        var socialLinks = command.SocialLinks
            .Select(x => SocialLink.Create(x.Name, x.Url))
            .Select(x => x.Value);

        volunteer.Value.UpdateSocialLinks(new ValueObjectList<SocialLink>(socialLinks));
        
        var resultUpdate = await repository.Save(volunteer.Value, token);
        if (resultUpdate.IsFailure)
            return resultUpdate.Error.ToErrorList();

        logger.Log(LogLevel.Information, "Volunteer {volunteerId} was updated social links", volunteerId);

        return resultUpdate.Value;
    }
}