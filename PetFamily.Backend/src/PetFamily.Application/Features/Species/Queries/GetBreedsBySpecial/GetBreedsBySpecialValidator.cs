using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.Species.Queries.GetBreedsBySpecial;

public class GetBreedsBySpecialValidator : AbstractValidator<GetBreedsBySpecialQuery>
{
    public GetBreedsBySpecialValidator()
    {
        RuleFor(s => s.SpecialId)
            .NotEmpty().WithError(Errors.General.ValueIsRequired("SpecialId"));
    }
}