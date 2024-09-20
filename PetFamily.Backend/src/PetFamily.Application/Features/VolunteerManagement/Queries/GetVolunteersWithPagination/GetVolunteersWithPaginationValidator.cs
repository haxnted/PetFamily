using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.VolunteerManagement.Queries.GetVolunteersWithPagination;

public class GetVolunteersWithPaginationValidator : AbstractValidator<GetVolunteersWithPaginationQuery>
{
    private readonly string[] validSortBy = new[] { "name", "surname", "age", "id" };

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