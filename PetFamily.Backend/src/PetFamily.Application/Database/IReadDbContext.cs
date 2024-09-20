using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Dto;

namespace PetFamily.Application.Database;

public interface IReadDbContext
{
    public IQueryable<VolunteerDto> Volunteers { get;  }
    public IQueryable<PetDto> Pets { get; }
}