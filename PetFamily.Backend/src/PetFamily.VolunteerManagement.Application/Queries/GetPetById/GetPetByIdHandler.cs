using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dto;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;

namespace PetFamily.VolunteerManagement.Application.Queries.GetPetById;

public class GetPetByIdHandler(
    IVolunteersReadDbContext readDbContext,
    IValidator<GetPetByIdQuery> validator) : IQueryHandler<PetDto, GetPetByIdQuery>
{
    public async Task<Result<PetDto, ErrorList>> Execute(GetPetByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(query, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToList();

        var pet = await readDbContext.Pets.FirstOrDefaultAsync(p => p.Id == query.PetId, cancellationToken);
        if (pet is null)
            return Errors.General.NotFound(query.PetId).ToErrorList();

        return pet;
    }
}