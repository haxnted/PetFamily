using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared;


namespace PetFamily.Infrastructure.Providers;

public class MinioProvider : IFileProvider
{
    private readonly IMinioClient _minioClient;
    private readonly ILogger<MinioProvider> _logger;

    public MinioProvider(IMinioClient minioClient, ILogger<MinioProvider> logger)
    {
        _minioClient = minioClient;
        _logger = logger;
    }

    public async Task<Result<string, Error>> Upload(FIleContent fIleContent,
        CancellationToken token = default)
    {
        try
        {
            var bucketExistsArgs = new BucketExistsArgs()
                .WithBucket("photos");

            var isBucketExists = await _minioClient.BucketExistsAsync(bucketExistsArgs, token);

            if (!isBucketExists)
            {
                var makeBucketArgs = new MakeBucketArgs()
                    .WithBucket("photos");

                await _minioClient.MakeBucketAsync(makeBucketArgs, token);
            }

            var putObjectsArgs = new PutObjectArgs()
                .WithBucket("photos")
                .WithStreamData(fIleContent.Stream)
                .WithObjectSize(fIleContent.Stream.Length)
                .WithObject(fIleContent.ObjectName);

            var result = _minioClient.PutObjectAsync(putObjectsArgs, token).Result;
            return result.ObjectName;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to upload file to minio");
            return Error.Failure("file.upload.error", "Failed to upload file to storage");
        }
    }

    public async Task<Result<string, Error>> Unload(string fileName, CancellationToken token = default)
    {
        var maxFileTimeAlive = 60 * 60 * 24;
        try
        {
            var presignedGetObjectArgs = new PresignedGetObjectArgs()
                .WithBucket("photos")
                .WithObject(fileName)
                .WithExpiry(maxFileTimeAlive);

            return await _minioClient.PresignedGetObjectAsync(presignedGetObjectArgs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate download link for file from Minio");
            return Error.Failure("file.unload.error", "Failed to generate download link for file from storage");
        }
    }

    public async Task<Result<string, Error>> Delete(string fileName, CancellationToken token = default)
    {
        try
        {
            var removeObjectArgs = new RemoveObjectArgs()
                .WithBucket("photos")
                .WithObject(fileName);

            await _minioClient.RemoveObjectAsync(removeObjectArgs, token);

            return fileName;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete file from Minio");
            return Error.Failure("file.delete.error", "Failed to delete file from storage");
        }
    }
}