using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Аggregate.Volunteer;

namespace PetFamily.Application.Volunteers;

public interface IVolunteersRepository
{
    public Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken = default);
    public Task<Result<Volunteer, Error>> GetByPhoneNumber(PhoneNumber requestNumber);
}