using RAG.Application.Interfaces;

namespace RAG.Workers.Ingestion;

public class MetadataWorker : BackgroundService
{
    private readonly ILogger<MetadataWorker> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public MetadataWorker(ILogger<MetadataWorker> logger, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Metadata worker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var metadataGenerator = scope.ServiceProvider.GetRequiredService<IMetadataGenerator>();

                // TODO: Pick up chunks needing metadata generation and process them

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in metadata worker.");
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }

        _logger.LogInformation("Metadata worker stopped.");
    }
}
