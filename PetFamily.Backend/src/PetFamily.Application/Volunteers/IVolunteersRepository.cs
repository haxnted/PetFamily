using CSharpFunctionalExtensions;
using PetFamily.Domain.Models;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers;

public interface IVolunteersRepository
{
    public Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken = default);
    public Task<Result<Volunteer, Error>> GetByPhoneNumber(PhoneNumber requestNumber);
}