﻿using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using PetFamily.Core.Providers.FileProvider;
using PetFamily.SharedKernel;

namespace PetFamily.VolunteerManagement.Infrastructure.Providers;

public class MinioProvider : IFileProvider
{
    private const int MAX_FILE_TIME_ALIVE = 60 * 60 * 24; // 24 hours
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
            .WithBucket(file.BucketName)
            .WithStreamData(file.Stream)
            .WithObjectSize(file.Stream.Length)
            .WithObject(file.File.Path);

        try
        {
            await _minioClient
                .PutObjectAsync(putObjectArgs, token);

            return file.File.Path;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Fail to upload File in minio with name {ObjectName}", file.File);

            return Error.Failure("File.upload", "Fail to upload File in minio");
        }
        finally
        {
            semaphoreSlim.Release();
        }
    }

    private async Task CreateBucketIfNotExists(IEnumerable<FileContent> fileContent, CancellationToken token)
    {
        HashSet<string> bucketNames = [..fileContent.Select(f => f.BucketName)];
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

    public async Task<Result<string, Error>> GetFileByName(string fileName, string bucketName, CancellationToken token = default)
    {
        try
        {
            var presignedGetObjectArgs = new PresignedGetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fileName)
                .WithExpiry(MAX_FILE_TIME_ALIVE);

            return await _minioClient.PresignedGetObjectAsync(presignedGetObjectArgs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate download link for File from Minio");
            return Error.Failure("File.unload.error", "Failed to generate download link for File from storage");
        }
    }

    public async Task<Result<string, Error>> Delete(string fileName, string bucketName, CancellationToken token = default)
    {
        try
        {
            var removeObjectArgs = new RemoveObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fileName);

            await _minioClient.RemoveObjectAsync(removeObjectArgs, token);

            return fileName;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete File from Minio");
            return Error.Failure("File.delete.error", "Failed to delete File from storage");
        }
    }
}