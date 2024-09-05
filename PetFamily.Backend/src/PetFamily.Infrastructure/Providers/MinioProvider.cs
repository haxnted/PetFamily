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
    private const int MAX_LIMIT_THREADS = 3;
    private readonly IMinioClient _minioClient;
    private readonly ILogger<MinioProvider> _logger;

    public MinioProvider(IMinioClient minioClient, ILogger<MinioProvider> logger)
    {
        _minioClient = minioClient;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<string>, Error>> UploadFiles(IEnumerable<FileContent> fileContent,
        CancellationToken token = default)
    {
        var fileContents = fileContent.ToList();
        var semaphoreSlim = new SemaphoreSlim(MAX_LIMIT_THREADS);
        try
        {
            await CreateBucketIfNotExists(fileContents, token);
            
            var tasks = fileContents
                .Select(async f => await AddFileToStorage(f, semaphoreSlim, token));

            var pathsResult = await Task.WhenAll(tasks);

            if (pathsResult.Any(p => p.IsFailure))
                return pathsResult.First().Error;

            var results = pathsResult.Select(p => p.Value).ToList();

            return results;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to upload files in minio");
            return Error.Failure("files.upload.error", "Failed to upload files to storage");
        }
    }

    private async Task<Result<string, Error>> AddFileToStorage(
        FileContent file,
        SemaphoreSlim semaphoreSlim,
        CancellationToken token)
    {
        await semaphoreSlim.WaitAsync(token);

        var putObjectArgs = new PutObjectArgs()
            .WithBucket("photos")
            .WithStreamData(file.Stream)
            .WithObjectSize(file.Stream.Length)
            .WithObject(file.ObjectName);

        try
        {
            await _minioClient
                .PutObjectAsync(putObjectArgs, token);

            return file.ObjectName;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Fail to upload file in minio with name {ObjectName}", file.ObjectName);

            return Error.Failure("file.upload", "Fail to upload file in minio");
        }
        finally
        {
            semaphoreSlim.Release();
        }
    }

    private async Task CreateBucketIfNotExists(IEnumerable<FileContent> fileContent, CancellationToken token)
    {
        HashSet<string> bucketNames = [..fileContent.Select(f => f.ObjectName)];
        foreach (var bucketName in bucketNames)
        {
            var bucketExistArgs = new BucketExistsArgs()
                .WithBucket(bucketName);

            var bucketExist = await _minioClient.BucketExistsAsync(bucketExistArgs, token);

            if (bucketExist == false)
            {
                var makeBucketArgs = new MakeBucketArgs()
                    .WithBucket(bucketName);

                await _minioClient.MakeBucketAsync(makeBucketArgs, token);
            }
        }
    }

    public async Task<Result<string, Error>> GetFileByName(string fileName, CancellationToken token = default)
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