using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Messaging;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteerManagement.ValueObjects;

namespace PetFamily.Infrastructure.BackgroundServices;

public class FilesCleanerBackgroundService : BackgroundService
{
    private readonly ILogger<FilesCleanerBackgroundService> _logger;
    private readonly IMessageQueue<IEnumerable<FilePath>> _messageQueue;
    private readonly IServiceScopeFactory _scopeFactory;

    public FilesCleanerBackgroundService(
        ILogger<FilesCleanerBackgroundService> logger,
        IMessageQueue<IEnumerable<FilePath>> messageQueue,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _messageQueue = messageQueue;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        var fileProvider = scope.ServiceProvider.GetRequiredService<IFileProvider>();

        while (!stoppingToken.IsCancellationRequested)
        {
            var files = await _messageQueue.ReadAsync(stoppingToken);

            foreach (var file in files)
            {
                await fileProvider.Delete(file.Path, Constants.BUCKET_NAME_FOR_PET_IMAGES, stoppingToken);
            }

            _logger.Log(LogLevel.Information, "Executed FilesCleanerBackground Service");
        }
    }
}