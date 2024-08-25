using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Аggregate.Volunteer;

namespace PetFamily.Application.Volunteers;

public interface IVolunteersRepository
{
    public Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken = default);

    public Task<Result<Guid, Error>> Update(VolunteerId id, FullName fullName,
        Description description, AgeExperience ageExperience, PhoneNumber phoneNumber,
        CancellationToken cancellationToken = default);

    public Task<Result<Guid, Error>> UpdateRequisites(VolunteerId id, RequisitesList requisitesList,
        CancellationToken cancellationToken = default);

    public Task<Result<Guid, Error>> UpdateSocialLinks(VolunteerId id, SocialLinksList socialLinksList,
        CancellationToken cancellationToken = default);

    public Task<Result<Volunteer, Error>> GetByPhoneNumber(PhoneNumber requestNumber,
        CancellationToken cancellationToken = default);

    public Task<Result<Volunteer, Error>> GetById(VolunteerId id, CancellationToken cancellationToken = default);
}