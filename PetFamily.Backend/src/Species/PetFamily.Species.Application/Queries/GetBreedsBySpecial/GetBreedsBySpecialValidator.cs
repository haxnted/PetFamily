using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.Species.Application.Queries.GetBreedsBySpecial;

public class GetBreedsBySpecialValidator : AbstractValidator<GetBreedsBySpecialQuery>
{
    public GetBreedsBySpecialValidator()
    {
        RuleFor(s => s.SpecialId)
            .NotEmpty().WithError(Errors.General.ValueIsRequired("SpecialId"));
    }
}