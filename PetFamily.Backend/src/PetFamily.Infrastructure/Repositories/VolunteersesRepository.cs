using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Volunteers;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Аggregate.Volunteer;

namespace PetFamily.Infrastructure.Repositories;

public class VolunteersRepository(ApplicationDbContext context) : IVolunteersRepository
{
    public async Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        await context.Volunteers.AddAsync(volunteer, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return volunteer.Id;
    }

    public async Task<Result<Guid, Error>> Update(VolunteerId id, FullName fullName,
        Description description, AgeExperience ageExperience, PhoneNumber phoneNumber,
        CancellationToken cancellationToken = default)
    {
        var updatedCount = await context.Volunteers
            .Where(v => v.Id == id)
            .ExecuteUpdateAsync(volunteer => volunteer
                    .SetProperty(v => v.FullName.Name, fullName.Name)
                    .SetProperty(v => v.FullName.Surname, fullName.Surname)
                    .SetProperty(v => v.FullName.Patronymic, fullName.Patronymic)
                    .SetProperty(v => v.GeneralDescription.Value, description.Value)
                    .SetProperty(v => v.AgeExperience.Years, ageExperience.Years)
                    .SetProperty(v => v.PhoneNumber.Value, phoneNumber.Value),
                cancellationToken);

        if (updatedCount == 0)
            return Errors.General.NotFound(id.Id);

        return id.Id;
    }

    public async Task<Result<Guid, Error>> UpdateRequisites(VolunteerId id, RequisitesList requisitesList,
        CancellationToken cancellationToken = default)
    {
        var volunteer = await context.Volunteers
            .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);

        if (volunteer == null)
            return Errors.General.NotFound();

        volunteer.UpdateRequisites(requisitesList);
        await context.SaveChangesAsync(cancellationToken);
        return id.Id;
    }


    public async Task<Result<Guid, Error>> UpdateSocialLinks(VolunteerId id, SocialLinksList socialLinksList,
        CancellationToken cancellationToken = default)
    {
        var volunteer = await context.Volunteers
            .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);

        if (volunteer == null)
            return Errors.General.NotFound();

        volunteer.UpdateSocialLinks(socialLinksList);
        await context.SaveChangesAsync(cancellationToken);

        return id.Id;
    }

    public async Task<Result<Volunteer, Error>> GetByPhoneNumber(PhoneNumber requestNumber,
        CancellationToken cancellationToken = default)
    {
        var volunteer =
            await context.Volunteers.FirstOrDefaultAsync(v => v.PhoneNumber == requestNumber, cancellationToken);

        if (volunteer == null)
            return Errors.General.NotFound();

        return volunteer;
    }

    public async Task<Result<Volunteer, Error>> GetById(VolunteerId id, CancellationToken cancellationToken = default)
    {
        var volunteer = await context.Volunteers
            .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);

        if (volunteer == null)
            return Errors.General.NotFound(id.Id);

        return volunteer;
    }
}