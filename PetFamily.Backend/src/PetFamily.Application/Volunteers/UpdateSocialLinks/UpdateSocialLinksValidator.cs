using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Аggregate.Volunteer;

namespace PetFamily.Application.Volunteers.UpdateSocialLinks;

public class UpdateSocialLinksValidator : AbstractValidator<UpdateSocialLinksRequest>
{
    public UpdateSocialLinksValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
        
        RuleForEach(c => c.SocialLinks)
            .MustBeValueObject(s => Requisite.Create(s.Name, s.Url));
    }
}