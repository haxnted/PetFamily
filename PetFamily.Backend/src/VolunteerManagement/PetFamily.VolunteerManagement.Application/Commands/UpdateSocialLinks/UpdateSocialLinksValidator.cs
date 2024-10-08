using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;
using PetFamily.VolunteerManagement.Domain.ValueObjects;

namespace PetFamily.VolunteerManagement.Application.Commands.UpdateSocialLinks;

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