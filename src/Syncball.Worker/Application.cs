using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Syncball.Worker;

public class Application : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<Application> _logger;

    public Application(IServiceProvider serviceProvider, ILogger<Application> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(1000, stoppingToken);
        }
    }
}
