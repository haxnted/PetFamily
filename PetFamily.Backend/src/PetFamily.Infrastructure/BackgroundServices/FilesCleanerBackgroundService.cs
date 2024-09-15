
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Messaging;
using PetFamily.Domain.VolunteerManagement.ValueObjects;

namespace PetFamily.Infrastructure.BackgroundServices;

public class FilesCleanerBackgroundService : BackgroundService
{
    private const string BUCKET_NAME = "files";
    private readonly ILogger<FilesCleanerBackgroundService> _logger;
    private readonly IMessageQueue<IEnumerable<FilePath>> _messageQueue;
    private readonly IFileProvider _fileProvider;
    
    public FilesCleanerBackgroundService(
        ILogger<FilesCleanerBackgroundService> logger,
        IMessageQueue<IEnumerable<FilePath>> messageQueue,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _messageQueue = messageQueue;

        using var scope = scopeFactory.CreateScope();
        _fileProvider = scope.ServiceProvider.GetRequiredService<IFileProvider>();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (stoppingToken.IsCancellationRequested != false)
        {
            var files = await _messageQueue.ReadAsync(stoppingToken);
            
            foreach (var file in files)
            {
                await _fileProvider.Delete(file.Path, BUCKET_NAME, stoppingToken);
            } 
            
            _logger.Log(LogLevel.Information, "Executed FilesCleanerBackground Service");
        }
    }
}