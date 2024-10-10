using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using PetFamily.Core.BackgroundServices;
using PetFamily.Core.MessageQueues;
using PetFamily.Core.Messaging;
using PetFamily.Core.Providers.FileProvider;
using PetFamily.VolunteerManagement.Application;
using PetFamily.VolunteerManagement.Domain.ValueObjects;
using PetFamily.VolunteerManagement.Infrastructure.Options;
using PetFamily.VolunteerManagement.Infrastructure.Providers;

namespace PetFamily.VolunteerManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddVolunteerInfrastructure(this IServiceCollection collection,
        IConfiguration configuration)
    {
        collection.AddScoped<VolunteersWriteDbContext>();
        collection.AddScoped<IVolunteersReadDbContext, VolunteersReadDbContext>();
        collection.AddScoped<IVolunteersRepository, VolunteersRepository>();
        
        collection.AddScoped<IVolunteerUnitOfWork, VolunteerUnitOfWork>();
        collection.AddMinioService(configuration);
        collection.AddSingleton<IMessageQueue<IEnumerable<FilePath>>, InMemoryMessageQueues<IEnumerable<FilePath>>>();
        collection.AddHostedService<FilesCleanerBackgroundService>();
        return collection;
    }

    private static IServiceCollection AddMinioService(this IServiceCollection collection,
        IConfiguration configuration)
    {
        collection.Configure<MinioOptions>(configuration.GetSection(MinioOptions.MINIO));

        collection.AddScoped<IFileProvider, MinioProvider>();

        collection.AddMinio(options =>
        {
            var minioOptions = configuration.GetSection(MinioOptions.MINIO).Get<MinioOptions>()
                               ?? throw new ApplicationException("Minio options not found");

            options.WithEndpoint(minioOptions.Endpoint)
                .WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey)
                .WithSSL(minioOptions.UseSSL);
        });
        return collection;
    }
}