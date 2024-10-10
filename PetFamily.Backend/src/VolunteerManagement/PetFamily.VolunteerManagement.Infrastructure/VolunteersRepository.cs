using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.EntityIds;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.VolunteerManagement.Application;
using PetFamily.VolunteerManagement.Domain;
using PetFamily.VolunteerManagement.Infrastructure.DbContexts;

namespace PetFamily.VolunteerManagement.Infrastructure;

public class VolunteersRepository(VolunteersWriteDbContext context) : IVolunteersRepository
{
    public async Task Add(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        await context.Volunteers.AddAsync(volunteer, cancellationToken);
    }

    public async Task Save(Volunteer volunteer,
        CancellationToken cancellationToken = default)
    {
        context.Volunteers.Attach(volunteer);
        await context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<Result<Volunteer, Error>> GetByPhoneNumber(PhoneNumber requestNumber,
        CancellationToken cancellationToken = default)
    {
        var volunteer = await context.Volunteers
            .Include(v => v.Pets)
            .FirstOrDefaultAsync(v => v.PhoneNumber == requestNumber, cancellationToken);

        if (volunteer == null)
            return Errors.General.NotFound();

        return volunteer;
    }

    public async Task<Result<Volunteer, Error>> GetById(VolunteerId id, CancellationToken cancellationToken = default)
    {
        var volunteer = await context.Volunteers
            .Include(v => v.Pets)
            .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);

        if (volunteer == null)
            return Errors.General.NotFound(id.Id);

        return volunteer;
    }
}