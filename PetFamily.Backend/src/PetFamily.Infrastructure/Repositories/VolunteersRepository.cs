using PetFamily.Domain.Models;

namespace PetFamily.Infrastructure.Repositories;

public class VolunteersRepository(ApplicationDbContext context)
{
    public async Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        await context.Volunteers.AddAsync(volunteer, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return volunteer.Id;
    }
}