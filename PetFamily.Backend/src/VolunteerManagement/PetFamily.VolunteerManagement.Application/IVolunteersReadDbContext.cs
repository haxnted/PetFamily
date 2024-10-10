using PetFamily.Core.Dto;

namespace PetFamily.VolunteerManagement.Application;

public interface IVolunteersReadDbContext
{
    public IQueryable<VolunteerDto> Volunteers { get; }
    public IQueryable<PetDto> Pets { get; }
}