using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Species;
using PetFamily.Domain.VolunteerManagement;

namespace PetFamily.Infrastructure.DbContexts;

public class WriteDbContext(IConfiguration configuration) : DbContext
{
    private const string DATABASE = "ApplcationDbContext";
    private const string PATH_CONTEXT = "Configurations.Write";
    
    public DbSet<Volunteer> Volunteers { get; set; }
    public DbSet<Species> Species { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(WriteDbContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSnakeCaseNamingConvention()
            .UseLoggerFactory(CreateLoggerFactory())
            .EnableSensitiveDataLogging()
            .UseNpgsql(configuration.GetConnectionString(DATABASE));
    }

    private ILoggerFactory CreateLoggerFactory()
    {
        return LoggerFactory.Create(builder =>
        {
            builder
                .AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Information)
                .AddConsole();
        });
    }
}