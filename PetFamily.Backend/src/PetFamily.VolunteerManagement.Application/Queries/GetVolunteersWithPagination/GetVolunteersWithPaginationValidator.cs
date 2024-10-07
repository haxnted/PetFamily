using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.VolunteerManagement.Application.Queries.GetVolunteersWithPagination;

public class GetVolunteersWithPaginationValidator : AbstractValidator<GetVolunteersWithPaginationQuery>
{
    public GetVolunteersWithPaginationValidator()
    {
        RuleFor(v => v.Page)
            .GreaterThanOrEqualTo(1)
            .WithError(Errors.General.ValueIsInvalid("Page"));
        
        RuleFor(v => v.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithError(Errors.General.ValueIsInvalid("PageSize"));

    }
}