using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.VolunteerManagement.Queries.GetAllPetsWithPagination;

public class GetAllPetsWithPaginationValidator : AbstractValidator<GetAllPetsWithPaginationQuery>
{
    public GetAllPetsWithPaginationValidator()
    {
        RuleFor(v => v.Page)
            .GreaterThanOrEqualTo(1)
            .WithError(Errors.General.ValueIsInvalid("Page"));
        
        RuleFor(v => v.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithError(Errors.General.ValueIsInvalid("PageSize"));
    }
}