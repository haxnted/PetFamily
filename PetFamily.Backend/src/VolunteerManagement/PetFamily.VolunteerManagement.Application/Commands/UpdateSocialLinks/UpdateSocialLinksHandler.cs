using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.EntityIds;
using PetFamily.VolunteerManagement.Domain.ValueObjects;

namespace PetFamily.VolunteerManagement.Application.Commands.UpdateSocialLinks;

public class UpdateSocialLinksHandler(
    IVolunteerUnitOfWork unitOfWork,
    IVolunteersRepository repository, 
    IValidator<UpdateSocialLinksCommand> validator,
    ILogger<UpdateSocialLinksHandler> logger) : ICommandHandler<Guid, UpdateSocialLinksCommand>
{
    public async Task<Result<Guid, ErrorList>> Execute(UpdateSocialLinksCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToList();
        
        var volunteerId = VolunteerId.Create(command.Id);
        var volunteer = await repository.GetById(volunteerId, cancellationToken);
        if (volunteer.IsFailure)
            return volunteer.Error.ToErrorList();

        var socialLinks = command.SocialLinks
            .Select(x => SocialLink.Create(x.Name, x.Url))
            .Select(x => x.Value)
            .ToList();

        volunteer.Value.UpdateSocialLinks(new List<SocialLink>(socialLinks));
        
        await repository.Save(volunteer.Value, cancellationToken);
        await unitOfWork.SaveChanges(cancellationToken);

        logger.Log(LogLevel.Information, "Volunteer {volunteerId} was updated social links", volunteerId);

        return volunteer.Value.Id.Id;
    }
}