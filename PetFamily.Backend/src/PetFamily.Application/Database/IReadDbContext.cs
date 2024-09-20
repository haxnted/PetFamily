using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Dto;

namespace PetFamily.Application.Database;

public interface IReadDbContext
{
    public DbSet<VolunteerDto> Volunteers { get; set; }
    public DbSet<PetDto> Pets { get; set; }
}