using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Volunteers;
using PetFamily.Domain.Models;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Repositories;

public class VolunteersesRepository(ApplicationDbContext context) : IVolunteersRepository
{
    public async Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        await context.Volunteers.AddAsync(volunteer, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return volunteer.Id;
    }

    public async Task<Result<Volunteer, Error>> GetByPhoneNumber(PhoneNumber requestNumber)
    {
        var volunteer = await context.Volunteers.FirstOrDefaultAsync(v => v.PhoneNumber == requestNumber);

        if (volunteer == null)
            return Errors.General.NotFound();

        return volunteer;
    }
}