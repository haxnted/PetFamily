﻿using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dto;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.EntityIds;

namespace PetFamily.VolunteerManagement.Application.Queries.GetVolunteerById;

public class GetVolunteerByIdHandler(
    IVolunteersReadDbContext context,
    ILogger<GetVolunteerByIdHandler> logger)
    : ICommandHandler<VolunteerDto, GetVolunteerByIdCommand>
{
    public async Task<Result<VolunteerDto, ErrorList>> Execute(
        GetVolunteerByIdCommand command,
        CancellationToken cancellationToken = default)
    {
        var volunteerId = VolunteerId.Create(command.Id);

        var volunteerDto = await context.Volunteers
            .FirstOrDefaultAsync(v => v.Id == volunteerId, cancellationToken);

        if (volunteerDto is null)
            return Errors.General.NotFound(volunteerId).ToErrorList();

        logger.Log(LogLevel.Information, "Get volunteer with Id {volunteerId}", volunteerId);
        return volunteerDto;
    }
}