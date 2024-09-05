﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using PetFamily.Application.Database;
using PetFamily.Application.Providers;
using PetFamily.Application.Volunteers;
using PetFamily.Infrastructure.Options;
using PetFamily.Infrastructure.Providers;
using PetFamily.Infrastructure.Repositories;

namespace PetFamily.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddAInfrastructure(this IServiceCollection collection,
        IConfiguration configuration)
    {
        collection.AddScoped<ApplicationDbContext>();
        collection.AddScoped<IVolunteersRepository, VolunteersRepository>();
        collection.AddScoped<IUnitOfWork, UnitOfWork>();
        collection.AddMinio(configuration);
        return collection;
    }

    private static IServiceCollection AddMinio(this IServiceCollection collection,
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