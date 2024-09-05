using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteerManagement.ValueObjects;

namespace PetFamily.Application.Features.Volunteers.UpdateSocialLinks;

public class UpdateSocialLinksValidator : AbstractValidator<UpdateSocialLinksCommand>
{
    public UpdateSocialLinksValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired("Id"));
        
        RuleForEach(c => c.SocialLinks)
            .MustBeValueObject(s => Requisite.Create(s.Name, s.Url));
    }
}