using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.EntityIds;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.VolunteerManagement.Domain;

namespace PetFamily.VolunteerManagement.Application;

public interface IVolunteersRepository
{
    public Task Add(Volunteer volunteer, CancellationToken cancellationToken = default);

    public Task Save(Volunteer volunteer, CancellationToken cancellationToken = default);
    
    public Task<Result<Volunteer, Error>> GetByPhoneNumber(PhoneNumber requestNumber,
        CancellationToken cancellationToken = default);

    public Task<Result<Volunteer, Error>> GetById(VolunteerId id, CancellationToken cancellationToken = default);
}